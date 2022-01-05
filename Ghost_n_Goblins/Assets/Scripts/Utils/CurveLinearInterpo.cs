namespace LinearInterpolation3D {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CurveLinearInterpo {
        public class LogPoint {
            public float length;
            public Vector3 point;

            public LogPoint(float lgt, Vector3 pt) {
                length = lgt;
                point = pt;
            }
        }

        List<LogPoint> _Points = new List<LogPoint>();
        List<int> _Indexes = new List<int>();
        float _TotalLength = 0;
        bool _isClosed;

        public delegate Vector3 MathFunction(float t);

        /// <summary>
        /// Constructor
        /// <c> CurveLinearInterpo </c>
        /// who calculate the array of points on a specific curve given as parameter. The step between each points is constant
        /// </summary>
        /// <param name="function"></param>
        /// <param name="tMin"></param>
        /// <param name="tMax"></param>
        /// <param name="nbPoints"></param>
        /// <param name="isClosed"></param>
        public CurveLinearInterpo(MathFunction function, float tMin, float tMax, int nbPoints, bool isClosed = false) {
            _isClosed = isClosed;
            Vector3 currentPoint;
            Vector3 previousPoint = function(tMin);
            int flooredTotalLength;
            float step = (tMax - tMin) / (nbPoints - 1);

            if (nbPoints < 2 || tMin == tMax) return;

            for (float j = 0; j <= nbPoints - 1; j++) {
                float t = tMin + step * j;
                currentPoint = function(t);
                _TotalLength += Vector3.Distance(currentPoint, previousPoint);
                flooredTotalLength = Mathf.FloorToInt(_TotalLength);

                for (int i = _Indexes.Count; i < flooredTotalLength; i++) {
                    _Indexes.Add(Mathf.Max(_Points.Count - 1, 0));
                }

                _Points.Add(new LogPoint(_TotalLength, currentPoint));
                previousPoint = currentPoint;
            }

            if (_isClosed) {
                LogPoint firstLogPoint = _Points[0];
                _TotalLength += Vector3.Distance(previousPoint, firstLogPoint.point);
                _Points.Add(new LogPoint(_TotalLength, firstLogPoint.point));
                flooredTotalLength = Mathf.FloorToInt(_TotalLength);

                for (int i = _Indexes.Count; i <= flooredTotalLength; i++) {
                    _Indexes.Add(Mathf.Max(_Points.Count - 1, 0));
                }
            }
        }

        /// <summary> 
        /// method 
        /// <c> GetPositionFromDistance </c>
        /// This function calculate the lineaire interpolation over a curve and calculates the point P at [dist] from the origin
        /// </summary>
        /// <param name="dist"> the distance to the origin </param>
        /// <returns> The point P on the curve at [dist] from the origin using the linear interpolation formula</returns>
        public Vector3 GetPositionFromDistance(float dist) {
            int flooredDistance;
            float distance = dist;
            int idx;
            LogPoint previousLogPoint;
            LogPoint nextLogPoint;
            if (_isClosed) {
                while (distance < 0) distance += _TotalLength;
                distance %= _TotalLength;
            }
            else distance = Mathf.Clamp(distance, 0, _TotalLength);
            flooredDistance = Mathf.FloorToInt(distance);
            idx = _Indexes[Mathf.Clamp(flooredDistance, 0, _Indexes.Count - 1)];
            while (_Points[idx].length < distance) idx++;
            if (idx > 0) idx = idx - 1;
            previousLogPoint = _Points[idx];
            nextLogPoint = _Points[idx + 1];

            return previousLogPoint.point + (nextLogPoint.point - previousLogPoint.point) * ((distance - previousLogPoint.length) / (nextLogPoint.length - previousLogPoint.length));
        }

        public int NumberOfPoints { get { return _Points == null ? 0 : _Points.Count; } }
        
        public float TotalLength { get { return _TotalLength; } }
    }

    public static class ParametricEquations {
        public static Vector3 Sin(float t) {
            return new Vector3(t, Mathf.Sin(t), 0);
        }

        public static Vector3 NegSin3(float t) {
            return new Vector3(-t, Mathf.Sin(t) * 3, 0);
        }

        public static Vector3 Lissajous(float t) {
            float a = 30;
            float b = 30;
            float theta = t % (2 * Mathf.PI);
            float phi = 0.2f;
            float p = 3;
            float q = 4;
            return new Vector3(a * Mathf.Sin(theta * p), b * Mathf.Sin(q * theta + phi), 0);
        }

        public static Vector3 Lemniscate(float t) {
            return new Vector3(Mathf.Sin(t) / (1 + Mathf.Pow(Mathf.Cos(t), 2)), Mathf.Sin(t) * Mathf.Cos(t) / (1 + Mathf.Pow(Mathf.Cos(t), 2)), 0);
        }
    }
}
