using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VRCFTnyan {
    internal class net2bodge {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Clamp(byte value, byte min, byte max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Clamp(decimal value, decimal min, decimal max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(double value, double min, double max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Clamp(short value, short min, short max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max) {
            if (min > max) {
                throw new Exception("MinMax");
                //ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }

            return value;
        }
    }
}
