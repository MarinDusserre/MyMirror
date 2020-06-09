#include <FastLED.h>

#define DATA_PIN 1
#define CLOCK_PIN 13

#define TOP_NUM_LEDS 30
#define BOT_NUM_LEDS 30
#define LEFT_NUM_LEDS 18
#define RIGHT_NUM_LEDS 18
#define NUM_LEDS (TOP_NUM_LEDS + BOT_NUM_LEDS + LEFT_NUM_LEDS + RIGHT_NUM_LEDS)
#define START_LED_NUM 18 // Top left LED index

#define INPUT_BUFFER_SIZE 128
#define LOOP_PERIODE_MS 50

#define LED_MAX_LUMINOSITY 90

enum ledState {
  Hidden,
  FadeIn,
  FadeOut,
  Shown,
  Waiting
};

typedef struct {
    ledState state;
    CRGB targetColor;
    CRGB startColor;
    int waitDuration;
    int fadeInDuration;
    int fadeOutDuration;
    int showDuration;
} Led_type;

int inputBuffer[INPUT_BUFFER_SIZE];
int inputBufferIndex = 0;
int lastFrame[INPUT_BUFFER_SIZE];

CRGB ledsCurrentColor[NUM_LEDS];
CRGB white;

Led_type leds[NUM_LEDS];

int lastLoopMillis = 0;
bool newFrame = false;
bool partyMode = false;
int blockUntil = 0;

void setup() {
    Serial.begin(9600);
    pinMode(LED_BUILTIN, OUTPUT);
    FastLED.addLeds<WS2812B, DATA_PIN, RGB>(ledsCurrentColor, NUM_LEDS);

    white.red =  0;
    white.green = 0;
    white.blue =  LED_MAX_LUMINOSITY / 2; 
}

void loop() { 
  int startLoopMillis = millis();

  // Parse input
  readSerialInput();
  parseInputBuffer();

  // Update target leds according to lastframe
  if(newFrame){
    parseNewFrame();
    newFrame = false;
  }
   
  // Update leds
  updateDisplayedLeds();

  // Set loop frequency
  int delta = LOOP_PERIODE_MS - (millis() - startLoopMillis);
  if(delta < 0)
  {
    delta = 0;
  }
  delay(delta);
}

void parseInputBuffer(){
  bool foundEnd = false;
  int tramSize = inputBuffer[1];
  if(tramSize != 0){
       if(inputBufferIndex >= tramSize)
       {
         for(int i = 0; i < tramSize; i++){
          lastFrame[i] = inputBuffer[i];
         }        
         clearBuffer(tramSize);

         if(blockUntil < millis()){
           newFrame = true;
         }
       }
   } 
}

void readSerialInput(){
  while(Serial.available() > 0) {
    inputBuffer[inputBufferIndex] = Serial.read();
    inputBufferIndex ++;
    if(inputBufferIndex > INPUT_BUFFER_SIZE){
      clearBuffer(INPUT_BUFFER_SIZE);
    }
  }
}

void clearBuffer(int clearRange){
  // Keep data of next frame
  for(int i = clearRange; i < INPUT_BUFFER_SIZE; i++){
      inputBuffer[i - clearRange] = inputBuffer[i];
  }
  // Clean the rest
  for(int i = INPUT_BUFFER_SIZE - clearRange; i < INPUT_BUFFER_SIZE; i++){
    inputBuffer[i] = 0;
  }
  inputBufferIndex -= clearRange;
  if(inputBufferIndex < 0){
    inputBufferIndex = 0;
  }
}
    
void parseNewFrame(){  
  int duration = 0;
  int indexes[4];
  switch (lastFrame[0]){
      case(1): // Start annimation  
        duration = lastFrame[2] * 256 + lastFrame[3];
        blockUntil = millis() + duration; 
        for(int i = 0; i < NUM_LEDS; i++){
          int index = realIndex(i);   
          leds[index].state = Waiting;
          leds[index].targetColor = white;
          leds[index].startColor = CRGB::Black;
          leds[index].fadeInDuration = 200 / LOOP_PERIODE_MS;
          leds[index].fadeOutDuration = 200 / LOOP_PERIODE_MS;
          leds[index].waitDuration = i * (duration / NUM_LEDS) / LOOP_PERIODE_MS ;
          leds[index].showDuration = (NUM_LEDS - i) * (duration / NUM_LEDS) /  LOOP_PERIODE_MS;
        }
        break;
      case(7): // Point finger
        duration = lastFrame[4] * 256 + lastFrame[5];
        indexes[0] = ((double)TOP_NUM_LEDS * ((double)lastFrame[2] / (double)100));// Top index
        indexes[1] = TOP_NUM_LEDS + ((double)RIGHT_NUM_LEDS * ((double)lastFrame[3] / (double)100));// Right Index       
        indexes[2] = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS - ((double)BOT_NUM_LEDS * ((double)lastFrame[2] / (double)100));// Bot Index     
        indexes[3] = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS + LEFT_NUM_LEDS - ((double)LEFT_NUM_LEDS * ((double)lastFrame[3] / (double)100));// Bot Index   
        
        for(int i = 0; i < 4; i++){ 
          int index = realIndex(indexes[i]);
          leds[index].state = Shown;
          leds[index].targetColor = CRGB::White;
          leds[index].startColor = CRGB::Black;
          leds[index].fadeOutDuration = 200 / LOOP_PERIODE_MS;
          leds[index].showDuration = 1000 /  LOOP_PERIODE_MS;
        }
        break; 
        case(8): // PartyMode
          partyMode = lastFrame[2] != 0;
        break; 
      default :
       int startIndex = -1;
       int endIndex = 0;
       switch (lastFrame[0]){
            case(2): // Show all
              startIndex = 0;
              endIndex = NUM_LEDS - 1;
              break; 
            case(3): // Show top
              startIndex = 0;
              endIndex = TOP_NUM_LEDS - 1;
              break; 
            case(4): // Show right
              startIndex = TOP_NUM_LEDS;
              endIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS - 1;
              break;   
            case(5): // Show bot
              startIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS;
              endIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS - 1;
              break;  
            case(6): // Show left 
              startIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS;
              endIndex = NUM_LEDS -1;
              break;      
       }
       if(startIndex != -1){     
          for(int i = startIndex; i <= endIndex; i++){
            int index = realIndex(i);  
            leds[index].state = FadeIn;
            leds[index].targetColor = white;
            leds[index].startColor = CRGB::Black;
            leds[index].fadeInDuration = (lastFrame[2] * 256 + lastFrame[3]) / LOOP_PERIODE_MS;
            leds[index].showDuration = (lastFrame[4] * 256 + lastFrame[5]) / LOOP_PERIODE_MS;
            leds[index].fadeOutDuration = (lastFrame[6] * 256 + lastFrame[7]) / LOOP_PERIODE_MS;
            if(leds[index].fadeInDuration != 0){
              leds[index].state = FadeIn;
            }
            else{
              leds[index].state = Shown;
            }
        }
       }
        break; 
    }    
}

void updateDisplayedLeds(){
  for(int i = 0; i < NUM_LEDS; i++){        
    int index = i;
    switch(leds[i].state){
      case (Waiting):
        leds[i].waitDuration--;
        if (leds[i].waitDuration <= 0){
          leds[i].state = FadeIn;
        }
        break;
      case (Hidden):      
        if(partyMode){
          if(i%5 == 0){
            ledsCurrentColor[index] = getNoise((i + START_LED_NUM) % NUM_LEDS);           
          }
        }
        else
        {
          ledsCurrentColor[index] = leds[i].startColor;
        }
        break;
      case (FadeIn):
        if (leds[i].fadeInDuration <= 0){
          leds[i].state = Shown;
        }      
        else
        {
          ledsCurrentColor[index].r = (byte)(((int)ledsCurrentColor[i].r) + (((int)leds[i].targetColor.r) - ((int)ledsCurrentColor[i].r)) / leds[i].fadeInDuration);
          ledsCurrentColor[index].g = (byte)(((int)ledsCurrentColor[i].g) + (((int)leds[i].targetColor.g) - ((int)ledsCurrentColor[i].g)) / leds[i].fadeInDuration);
          ledsCurrentColor[index].b = (byte)(((int)ledsCurrentColor[i].b) + (((int)leds[i].targetColor.b) - ((int)ledsCurrentColor[i].b)) / leds[i].fadeInDuration);  
          leds[i].fadeInDuration--;
        }
        break;        
      case (FadeOut):
         if (leds[i].fadeOutDuration <= 0){
          leds[i].state = Hidden;
        }
        else
        {
          ledsCurrentColor[index].r = (byte)(((int)ledsCurrentColor[i].r) + (((int)leds[i].startColor.r) - ((int)ledsCurrentColor[i].r)) / leds[i].fadeOutDuration);
          ledsCurrentColor[index].g = (byte)(((int)ledsCurrentColor[i].g) + (((int)leds[i].startColor.g) - ((int)ledsCurrentColor[i].g)) / leds[i].fadeOutDuration);
          ledsCurrentColor[index].b = (byte)(((int)ledsCurrentColor[i].b) + (((int)leds[i].startColor.b) - ((int)ledsCurrentColor[i].b)) / leds[i].fadeOutDuration);
          leds[i].fadeOutDuration--;
        }
        break;
      case (Shown):
        ledsCurrentColor[index] = leds[i].targetColor;
        leds[i].showDuration--;
        if (leds[i].showDuration <= 0){
            if(leds[i].fadeOutDuration != 0){
              leds[i].state = FadeOut;
            }
            else{
              leds[i].state = Hidden;
            }
        }
        break;           
    }
  }
  FastLED.show();
}

int realIndex(int index){
  return (START_LED_NUM + TOP_NUM_LEDS - index + NUM_LEDS - 1)% NUM_LEDS;
}

CRGB getNoise(int ledIndex){
  int scale = 30; 
  int xPos, yPos; 

  if(ledIndex < TOP_NUM_LEDS){
    xPos = ledIndex;
    yPos = 0;
  }
  else if(ledIndex < (TOP_NUM_LEDS + BOT_NUM_LEDS))
  {
    xPos = TOP_NUM_LEDS;
    yPos = ledIndex - TOP_NUM_LEDS + 1;
  }
  else if(ledIndex < (TOP_NUM_LEDS + BOT_NUM_LEDS + LEFT_NUM_LEDS))
  {
    xPos = NUM_LEDS - RIGHT_NUM_LEDS - ledIndex - 1;;
    yPos = LEFT_NUM_LEDS;
  }
  else
  {
    xPos = 0;
    yPos = NUM_LEDS - ledIndex - 1;
  }

  int noise = inoise8(scale * xPos, scale * yPos, (millis() % 65536) / 5);
  return CHSV(noise, LED_MAX_LUMINOSITY, noise);
}
