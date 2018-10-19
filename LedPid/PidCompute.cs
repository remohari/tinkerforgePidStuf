using System;

namespace LedPid
{
    internal class PidCompute
    {
        private DateTime lastTime;
        private double errSum, lastErr;
        private double kp, ki, kd;
        private double errSumMax = 1025500000;

        public PidCompute(double kp, double ki, double kd)
        {
            this.kp = kp;
            this.ki = ki;
            this.kd = kd;
        }

        public double Compute(double setpoint, double input)
        {
            /*How long since we last calculated*/
            var now = DateTime.Now;
            var timeChange = (double)(now.Millisecond - lastTime.Millisecond);

            /*Compute all the working error variables*/
            var error = setpoint - input;

            errSum += (error * timeChange);
            errSum = Math.Min(errSum, errSumMax);
            errSum = Math.Max(errSum, -errSumMax);

            /*if (errSum + (error * timeChange) < errSumMax || errSum + (error * timeChange) > -errSumMax)
            {
                errSum += (error * timeChange);
            }*/
            var dErr = (error - lastErr); // timeChange;

            /*Remember some variables for next time*/
            lastErr = error;
            lastTime = now;

            return kp * error + ki * errSum + kd * dErr;
        }

        public void SetTunings(double Kp, double Ki, double Kd)
        {
            kp = Kp;
            ki = Ki;
            kd = Kd;
        }
    }
}