// -----------------------------------------------------------------------
// <copyright file="Stabilizator.cs">
//
// </copyright>
// <summary>Stabilizator/summary>
// -----------------------------------------------------------------------


namespace LeapMotionInput
{
    using Common.Log;
    using Leap;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Manage leapmotion stabilization
    /// </summary>
    public class Stabilizator
    {
        /// <summary>
        /// API access mutex
        /// </summary>
        private readonly Mutex _accessMutex;

        /// <summary>
        /// Lasts postions
        /// </summary>
        private List<Vector> _lastPositions;

        /// <summary>
        /// Buffer size
        /// </summary>
        private int _bufferSize;   

        /// <summary>
        /// Time stamp
        /// </summary>
        private int _timeStamp;     

        public Stabilizator(int bufferSize, int timeStamp)
        {
            _accessMutex = new Mutex();

            _bufferSize = bufferSize;
            _timeStamp = timeStamp;

            _lastPositions = new List<Vector>();
            _lastPositions.AddRange(Enumerable.Repeat(new Vector(0,0,0), bufferSize));
        }

        /// <summary>
        /// Set last position
        /// </summary>
        /// <param name="position">Last postion</param>
        /// <param name="time">Time of the last position</param>
        public void SetLastPostion(Vector position, DateTime time)
        {
            if(_accessMutex.WaitOne(30))
            {
                _lastPositions.RemoveAt(_bufferSize - 1);
                _lastPositions.Insert(0, position);
            }
            else
            {
                LogManager.LogLine("SetLastPostion timeout");
            }
        }

        /// <summary>
        /// Get last corrected position
        /// </summary>
        /// <returns>Corrected position</returns>
        public Vector GetLastCorrectedPosition()
        {
            Vector ret = null;
            
            if (_accessMutex.WaitOne(100))
            {
                // Get average pos
                int nbAverages = 2;
                Vector[] averagePoints = new Vector[nbAverages];

                int blockSize = _bufferSize / nbAverages;
                
                for (int i = 0; i < nbAverages; i++)
                {
                    averagePoints[i] = new Vector(0, 0, 0);
                    for (int j = 0; j < blockSize; i++)
                    {
                        averagePoints[i] += _lastPositions[j + i * blockSize] ?? new Vector(0, 0, 0);
                    }
                    averagePoints[i]  = averagePoints[i] / nbAverages;
                }

                // Get slope
                int[] times = new int[nbAverages];
                for (int i = 0; i < nbAverages; i++)
                {
                    times[i] = (int)(_timeStamp * blockSize * (0.5f + i));
                }
                Vector slope = - (averagePoints[1] - averagePoints[0]) / (times[1] - times[0]);

                // Interpolate
                ret = averagePoints[0] + new Vector(averagePoints[0].x * slope.x, averagePoints[0].y * slope.y, averagePoints[0].z * slope.z);
   
                _accessMutex.ReleaseMutex();
            }
            else
            {
                LogManager.LogLine("GetLastCorrectedPosition timeout");
            }
            return ret;
        }
    }
}
