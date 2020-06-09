#include <ESP8266WiFi.h>
#include <ESP8266WebServer.h>
#include <FastLED.h>

#define STASSID "Freebox-0C24A2"
#define STAPSK  "pignoribus2-frustilla?9-fugata-ligonibus"

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

#define LED_MAX_LUMINOSITY 150

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

bool partyMode = false;

int inputBuffer[INPUT_BUFFER_SIZE];
int inputBufferIndex = 0;
int lastFrame[INPUT_BUFFER_SIZE];
int lastLoopMillis = 0;
int blockUntil = 0;

ESP8266WebServer server(80);

CRGB ledsCurrentColor[NUM_LEDS];
CRGB white;

Led_type leds[NUM_LEDS];

void setup() {
  Serial.begin(9600);
  pinMode(LED_BUILTIN, OUTPUT);
  FastLED.addLeds<WS2812B, DATA_PIN, RGB>(ledsCurrentColor, NUM_LEDS);

  white.red =  LED_MAX_LUMINOSITY;
  white.green = LED_MAX_LUMINOSITY;
  white.blue =  LED_MAX_LUMINOSITY; 

  WiFi.begin(STASSID, STAPSK);
    while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Waiting to connect...");
  }
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  server.begin();
  server.on("/leds", handleRequest);
}

void loop() { 
  int startLoopMillis = millis();
  
  // Handke server request 
  server.handleClient();

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
    
void handleRequest(){  
  int duration = 0;
  int indexes[4];
  int argsNumber = server.args();
  if(argsNumber > 0 && server.argName(0).equals("request")) {  
    server.send(200, "text/plain", "Request received : " + server.arg(0));
    if(server.arg(0).equals("initialization")) {
      duration = 10000;
      if(argsNumber > 1 && server.argName(1).equals("diplayduration"))
      {
        duration = server.arg(1).toInt();
      }
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
    } 
    else if(server.arg(0).equals("position")) {
      duration = 1000;
      int x = 0;
      int y = 0;
      if(argsNumber > 2)
      {
        duration = server.arg(1).toInt();
        x = server.arg(2).toInt();
        y = server.arg(3).toInt();
      }
      indexes[0] = ((double)TOP_NUM_LEDS * ((double)x/ (double)100));// Top index
      indexes[1] = TOP_NUM_LEDS + ((double)RIGHT_NUM_LEDS * ((double)y / (double)100));// Right Index       
      indexes[2] = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS - ((double)BOT_NUM_LEDS * ((double)x / (double)100));// Bot Index     
      indexes[3] = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS + LEFT_NUM_LEDS - ((double)LEFT_NUM_LEDS * ((double)y / (double)100));// Left Index   
    
      for(int i = 0; i < 4; i++){ 
        int index = realIndex(indexes[i]);
        leds[index].state = Shown;
        leds[index].targetColor = CRGB::White;
        leds[index].startColor = CRGB::Black;
        leds[index].fadeOutDuration = 200 / LOOP_PERIODE_MS;
        leds[index].showDuration = 1000 /  LOOP_PERIODE_MS;
      }
    } 
    else if(server.arg(0).equals("partymode")) {
      if(argsNumber > 1)
      {
        partyMode = server.arg(1).toInt() != 0;
      }
    } 
    else {   
      int startIndex = -1;
      int endIndex = 0;
      if(server.arg(0).equals("showall")) {
        startIndex = 0;
        endIndex = NUM_LEDS - 1;
      }
      else if(server.arg(0).equals("showtop")){
        startIndex = 0;
        endIndex = TOP_NUM_LEDS - 1;
      }
      else if(server.arg(0).equals("showright")){
        startIndex = TOP_NUM_LEDS;
        endIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS - 1;
      }
      else if(server.arg(0).equals("showbot")){
        startIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS;
        endIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS - 1;
      }
      else if(server.arg(0).equals("showleft")){
        startIndex = TOP_NUM_LEDS + RIGHT_NUM_LEDS + BOT_NUM_LEDS;
        endIndex = NUM_LEDS -1;
      }     
      if(startIndex != -1){     
        int fadeInDuration = 1000;
        int showDuration = 1000;
        int fadeOutDuration = 1000;  
        if(argsNumber > 2)
        {
          fadeInDuration = server.arg(1).toInt();
          showDuration = server.arg(2).toInt();
          fadeOutDuration = server.arg(3).toInt();
        }
        for(int i = startIndex; i <= endIndex; i++){
          int index = realIndex(i);  
          leds[index].state = FadeIn;
          leds[index].targetColor = white;
          leds[index].startColor = CRGB::Black;
          leds[index].fadeInDuration = fadeInDuration / LOOP_PERIODE_MS;
          leds[index].showDuration = showDuration / LOOP_PERIODE_MS;
          leds[index].fadeOutDuration = fadeOutDuration / LOOP_PERIODE_MS;
          if(leds[index].fadeInDuration != 0){
            leds[index].state = FadeIn;
          }
          else{
            leds[index].state = Shown;
          }
        }
      }
    }
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
            ledsCurrentColor[index] = getNoise((i + START_LED_NUM) % NUM_LEDS);  
        }
        else {
          ledsCurrentColor[index] = leds[i].startColor;
        }
        break;
      case (FadeIn):
        if (leds[i].fadeInDuration <= 0){
          leds[i].state = Shown;
        }      
        else {
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
        else {
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
            else {
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
  int scale = 10; 
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

  int noise = inoise8(scale * xPos, scale * yPos, (millis() % 65536) / 3);
  return CHSV(noise, 255, noise);
}
