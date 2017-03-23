using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptRuntime
{
    [Serializable]
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;

        public float Y;

        public float Z;

        private static Vector3 _zero;

        private static Vector3 _one;

        private static Vector3 _unitX;

        private static Vector3 _unitY;

        private static Vector3 _unitZ;

        private static Vector3 _up;

        private static Vector3 _down;

        private static Vector3 _right;

        private static Vector3 _left;

        private static Vector3 _forward;

        private static Vector3 _backward;

        private static float k1OverSqrt2;

        private static readonly float epsilon;

        public static Vector3 Zero
        {
            get
            {
                return Vector3._zero;
            }
        }

        public static Vector3 One
        {
            get
            {
                return Vector3._one;
            }
        }

        public static Vector3 UnitX
        {
            get
            {
                return Vector3._unitX;
            }
        }

        public static Vector3 UnitY
        {
            get
            {
                return Vector3._unitY;
            }
        }

        public static Vector3 UnitZ
        {
            get
            {
                return Vector3._unitZ;
            }
        }

        public static Vector3 Up
        {
            get
            {
                return Vector3._up;
            }
        }

        public static Vector3 Down
        {
            get
            {
                return Vector3._down;
            }
        }

        public static Vector3 Right
        {
            get
            {
                return Vector3._right;
            }
        }

        public static Vector3 Left
        {
            get
            {
                return Vector3._left;
            }
        }

        public static Vector3 Forward
        {
            get
            {
                return Vector3._forward;
            }
        }

        public static Vector3 Backward
        {
            get
            {
                return Vector3._backward;
            }
        }

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(float value)
        {
            this.Z = value;
            this.Y = value;
            this.X = value;
        }

        public Vector3(Vector2 value, float z)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = z;
        }

        //public override string ToString()
        //{
        //    CultureInfo currentCulture = CultureInfo.CurrentCulture;
        //    return string.Format(currentCulture, "{0}, {1}, {2}", new object[]
        //    {
        //        this.X.ToString(currentCulture),
        //        this.Y.ToString(currentCulture),
        //        this.Z.ToString(currentCulture)
        //    });
        //}

        public bool Equals(Vector3 other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Vector3)
            {
                result = this.Equals((Vector3)obj);
            }
            return result;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode();
        }

        public float Length()
        {
            float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            return (float)Math.Sqrt((double)num);
        }

        public float LengthSquared()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        public static float Distance(Vector3 value1, Vector3 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            return (float)Math.Sqrt((double)num4);
        }

        public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            result = (float)Math.Sqrt((double)num4);
        }

        public static float DistanceSquared(Vector3 value1, Vector3 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            return num * num + num2 * num2 + num3 * num3;
        }

        public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            result = num * num + num2 * num2 + num3 * num3;
        }

        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
        {
            result = vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public void Normalize()
        {
            float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
            if (num < Vector3.epsilon)
            {
                return;
            }
            float num2 = 1f / (float)Math.Sqrt((double)num);
            this.X *= num2;
            this.Y *= num2;
            this.Z *= num2;
        }

        public static Vector3 Normalize(Vector3 value)
        {
            float num = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
            if (num < Vector3.epsilon)
            {
                return value;
            }
            float num2 = 1f / (float)Math.Sqrt((double)num);
            Vector3 result;
            result.X = value.X * num2;
            result.Y = value.Y * num2;
            result.Z = value.Z * num2;
            return result;
        }

        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            float num = value.X * value.X + value.Y * value.Y + value.Z * value.Z;
            if (num < Vector3.epsilon)
            {
                result = value;
                return;
            }
            float num2 = 1f / (float)Math.Sqrt((double)num);
            result.X = value.X * num2;
            result.Y = value.Y * num2;
            result.Z = value.Z * num2;
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            Vector3 result;
            result.X = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            result.Y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            result.Z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            return result;
        }

        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            float x = vector1.Y * vector2.Z - vector1.Z * vector2.Y;
            float y = vector1.Z * vector2.X - vector1.X * vector2.Z;
            float z = vector1.X * vector2.Y - vector1.Y * vector2.X;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
        {
            float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            Vector3 result;
            result.X = vector.X - 2f * num * normal.X;
            result.Y = vector.Y - 2f * num * normal.Y;
            result.Z = vector.Z - 2f * num * normal.Z;
            return result;
        }

        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            float num = vector.X * normal.X + vector.Y * normal.Y + vector.Z * normal.Z;
            result.X = vector.X - 2f * num * normal.X;
            result.Y = vector.Y - 2f * num * normal.Y;
            result.Z = vector.Z - 2f * num * normal.Z;
        }

        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = ((value1.X < value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
            return result;
        }

        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = ((value1.X < value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y < value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z < value2.Z) ? value1.Z : value2.Z);
        }

        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = ((value1.X > value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
            return result;
        }

        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = ((value1.X > value2.X) ? value1.X : value2.X);
            result.Y = ((value1.Y > value2.Y) ? value1.Y : value2.Y);
            result.Z = ((value1.Z > value2.Z) ? value1.Z : value2.Z);
        }

        public static Vector3 Clamp(Vector3 value1, Vector3 min, Vector3 max)
        {
            float num = value1.X;
            num = ((num > max.X) ? max.X : num);
            num = ((num < min.X) ? min.X : num);
            float num2 = value1.Y;
            num2 = ((num2 > max.Y) ? max.Y : num2);
            num2 = ((num2 < min.Y) ? min.Y : num2);
            float num3 = value1.Z;
            num3 = ((num3 > max.Z) ? max.Z : num3);
            num3 = ((num3 < min.Z) ? min.Z : num3);
            Vector3 result;
            result.X = num;
            result.Y = num2;
            result.Z = num3;
            return result;
        }

        public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            float num = value1.X;
            num = ((num > max.X) ? max.X : num);
            num = ((num < min.X) ? min.X : num);
            float num2 = value1.Y;
            num2 = ((num2 > max.Y) ? max.Y : num2);
            num2 = ((num2 < min.Y) ? min.Y : num2);
            float num3 = value1.Z;
            num3 = ((num3 > max.Z) ? max.Z : num3);
            num3 = ((num3 < min.Z) ? min.Z : num3);
            result.X = num;
            result.Y = num2;
            result.Z = num3;
        }

        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            Vector3 result;
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return result;
        }

        public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
        }

        public static Vector3 SmoothStep(Vector3 value1, Vector3 value2, float amount)
        {
            amount = ((amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount));
            amount = amount * amount * (3f - 2f * amount);
            Vector3 result;
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return result;
        }

        public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            amount = ((amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount));
            amount = amount * amount * (3f - 2f * amount);
            result.X = value1.X + (value2.X - value1.X) * amount;
            result.Y = value1.Y + (value2.Y - value1.Y) * amount;
            result.Z = value1.Z + (value2.Z - value1.Z) * amount;
        }

        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = 2f * num2 - 3f * num + 1f;
            float num4 = -2f * num2 + 3f * num;
            float num5 = num2 - 2f * num + amount;
            float num6 = num2 - num;
            Vector3 result;
            result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
            result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
            result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
            return result;
        }

        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = 2f * num2 - 3f * num + 1f;
            float num4 = -2f * num2 + 3f * num;
            float num5 = num2 - 2f * num + amount;
            float num6 = num2 - num;
            result.X = value1.X * num3 + value2.X * num4 + tangent1.X * num5 + tangent2.X * num6;
            result.Y = value1.Y * num3 + value2.Y * num4 + tangent1.Y * num5 + tangent2.Y * num6;
            result.Z = value1.Z * num3 + value2.Z * num4 + tangent1.Z * num5 + tangent2.Z * num6;
        }

        public static Vector3 Negate(Vector3 value)
        {
            Vector3 result;
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            return result;
        }

        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }

        public static Vector3 Add(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }

        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }

        public static Vector3 Sub(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
            return result;
        }

        public static void Sub(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }

        public static Vector3 Multiply(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
            return result;
        }

        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }

        public static Vector3 Multiply(Vector3 value1, float scaleFactor)
        {
            Vector3 result;
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
            return result;
        }

        public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }

        public static Vector3 Divide(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
            return result;
        }

        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        public static Vector3 Divide(Vector3 value1, float divider)
        {
            float num = 1f / divider;
            Vector3 result;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
            return result;
        }

        public static void Divide(ref Vector3 value1, float divider, out Vector3 result)
        {
            float num = 1f / divider;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
        }

        private static float magnitude(ref Vector3 inV)
        {
            return (float)Math.Sqrt((double)Vector3.Dot(inV, inV));
        }

        private static Vector3 orthoNormalVectorFast(ref Vector3 n)
        {
            Vector3 result;
            if (Math.Abs(n.Z) > Vector3.k1OverSqrt2)
            {
                float num = n.Y * n.Y + n.Z * n.Z;
                float num2 = 1f / (float)Math.Sqrt((double)num);
                result.X = 0f;
                result.Y = -n.Z * num2;
                result.Z = n.Y * num2;
            }
            else
            {
                float num3 = n.X * n.X + n.Y * n.Y;
                float num4 = 1f / (float)Math.Sqrt((double)num3);
                result.X = -n.Y * num4;
                result.Y = n.X * num4;
                result.Z = 0f;
            }
            return result;
        }

        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent)
        {
            float num = Vector3.magnitude(ref normal);
            if (num > Vector3.epsilon)
            {
                normal /= num;
            }
            else
            {
                normal = new Vector3(1f, 0f, 0f);
            }
            float scaleFactor = Vector3.Dot(normal, tangent);
            tangent -= scaleFactor * normal;
            num = Vector3.magnitude(ref tangent);
            if (num < Vector3.epsilon)
            {
                tangent = Vector3.orthoNormalVectorFast(ref normal);
                return;
            }
            tangent /= num;
        }

        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent, ref Vector3 binormal)
        {
            float num = Vector3.magnitude(ref normal);
            if (num > Vector3.epsilon)
            {
                normal /= num;
            }
            else
            {
                normal = new Vector3(1f, 0f, 0f);
            }
            float scaleFactor = Vector3.Dot(normal, tangent);
            tangent -= scaleFactor * normal;
            num = Vector3.magnitude(ref tangent);
            if (num > Vector3.epsilon)
            {
                tangent /= num;
            }
            else
            {
                tangent = Vector3.orthoNormalVectorFast(ref normal);
            }
            float scaleFactor2 = Vector3.Dot(tangent, binormal);
            scaleFactor = Vector3.Dot(normal, binormal);
            binormal -= scaleFactor * normal + scaleFactor2 * tangent;
            num = Vector3.magnitude(ref binormal);
            if (num > Vector3.epsilon)
            {
                binormal /= num;
                return;
            }
            binormal = Vector3.Cross(normal, tangent);
        }

        public static Vector3 Project(Vector3 vector, Vector3 onNormal)
        {
            return onNormal * Vector3.Dot(vector, onNormal) / Vector3.Dot(onNormal, onNormal);
        }

        public static void Project(ref Vector3 vector, ref Vector3 onNormal, out Vector3 result)
        {
            result = onNormal * Vector3.Dot(vector, onNormal) / Vector3.Dot(onNormal, onNormal);
        }

        //public static float Angle(Vector3 from, Vector3 to)
        //{
        //    from.Normalize();
        //    to.Normalize();
        //    float value;
        //    Vector3.Dot(ref from, ref to, out value);
        //    return MathHelper.ACos(MathHelper.Clamp(value, -1f, 1f)) * 57.29578f;
        //}

        //public static void Angle(ref Vector3 from, ref Vector3 to, out float result)
        //{
        //    from.Normalize();
        //    to.Normalize();
        //    float value;
        //    Vector3.Dot(ref from, ref to, out value);
        //    result = MathHelper.ACos(MathHelper.Clamp(value, -1f, 1f)) * 57.29578f;
        //}

        public static Vector3 operator -(Vector3 value)
        {
            Vector3 result;
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            return result;
        }

        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
        }

        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
        }

        public static Vector3 operator +(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
            return result;
        }

        public static Vector3 operator -(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
            return result;
        }

        public static Vector3 operator *(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
            return result;
        }

        public static Vector3 operator *(Vector3 value, float scaleFactor)
        {
            Vector3 result;
            result.X = value.X * scaleFactor;
            result.Y = value.Y * scaleFactor;
            result.Z = value.Z * scaleFactor;
            return result;
        }

        public static Vector3 operator *(float scaleFactor, Vector3 value)
        {
            Vector3 result;
            result.X = value.X * scaleFactor;
            result.Y = value.Y * scaleFactor;
            result.Z = value.Z * scaleFactor;
            return result;
        }

        public static Vector3 operator /(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
            return result;
        }

        public static Vector3 operator /(Vector3 value, float divider)
        {
            float num = 1f / divider;
            Vector3 result;
            result.X = value.X * num;
            result.Y = value.Y * num;
            result.Z = value.Z * num;
            return result;
        }

        static Vector3()
        {
            Vector3.k1OverSqrt2 = 0.707106769f;
            Vector3.epsilon = 1E-05f;
            Vector3._zero = default(Vector3);
            Vector3._one = new Vector3(1f, 1f, 1f);
            Vector3._unitX = new Vector3(1f, 0f, 0f);
            Vector3._unitY = new Vector3(0f, 1f, 0f);
            Vector3._unitZ = new Vector3(0f, 0f, 1f);
            Vector3._up = new Vector3(0f, 1f, 0f);
            Vector3._down = new Vector3(0f, -1f, 0f);
            Vector3._right = new Vector3(1f, 0f, 0f);
            Vector3._left = new Vector3(-1f, 0f, 0f);
            Vector3._forward = new Vector3(0f, 0f, -1f);
            Vector3._backward = new Vector3(0f, 0f, 1f);
        }
    }
}
