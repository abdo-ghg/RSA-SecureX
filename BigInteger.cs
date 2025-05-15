using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    internal class BigInteger
        {
            public List<byte> digits; // digits stored in little-endian order (LSB first)

            //El Code Da b3mel override l Logical operators zy : == , >= , !=, << , >> , & 
            public static bool operator <(BigInteger a, BigInteger b) => a.compare(b) < 0;
            public static bool operator >(BigInteger a, BigInteger b) => a.compare(b) > 0;
            public static bool operator ==(BigInteger a, BigInteger b) => a.compare(b) == 0;
            public static bool operator !=(BigInteger a, BigInteger b) => !(a == b);
            public static bool operator <=(BigInteger a, BigInteger b) => a.compare(b) <= 0;
            public static bool operator >=(BigInteger a, BigInteger b) => a.compare(b) >= 0;
            public static BigInteger operator &(BigInteger a, BigInteger b)
            {
                if (b.digits.Count == 1 && b.digits[0] == 1)
                {
                    return new BigInteger(a.digits[0] & 1);
                }

                throw new NotImplementedException("Bitwise AND only implemented for (BigInteger & 1)");
            }
            public static BigInteger operator >>(BigInteger a, int shift)
            {
                BigInteger result = new BigInteger(a);

                for (int i = 0; i < shift; i++)
                {
                    result = floor(result, new BigInteger(2));
                }

                return result;
            }
            public static BigInteger operator <<(BigInteger a, int shift)
            {
                BigInteger result = new BigInteger(a);

                for (int i = 0; i < shift; i++)
                {
                    result = Multiply(result, new BigInteger(2));
                }

                return result;
            }

            // the constructors
            public BigInteger()
            {
                digits = new List<byte>() { 0 };
            }

            public BigInteger(BigInteger a)
            {
                digits = a.digits;
            }


            public BigInteger(string number)
            {

                digits = new List<byte>();
                foreach (char c in number.Reverse())
                {

                    digits.Add((byte)(c - '0'));
                }

                removeZeros();
                if (digits.Count == 0)
                    digits.Add(0);
            }
            public BigInteger(List<byte> digits)
            {
                this.digits = new List<byte>(digits);
                removeZeros();
                if (this.digits.Count == 0)
                    this.digits.Add(0);
            }
            public BigInteger(int num)
            {
                digits = new List<byte>();
                if (num == 0)
                {
                    digits.Add(0);

                }

                while (num > 0)
                {
                    digits.Add((byte)(num % 10));
                    num /= 10;
                }

            }

            // shyl alasfar
            private void removeZeros()
            {
                for (int i = digits.Count - 1; i > 0; i--)
                {
                    if (digits[i] == 0)
                        digits.RemoveAt(i);
                    else
                        break;
                }
            }

            // 3shan a3ml override l operators zy + , - , * , /
            public static BigInteger Add(BigInteger firstNum, BigInteger secondNum)
            {
                List<byte> result = new List<byte>();
                int maxLength = Math.Max(firstNum.digits.Count, secondNum.digits.Count);
                int temp = 0;
                byte carry = 0;
                // 3shan a3ml l + operator
                for (int i = 0; i < maxLength; i++)
                {
                    byte digitA = i < firstNum.digits.Count ? firstNum.digits[i] : (byte)0;
                    byte digitB = i < secondNum.digits.Count ? secondNum.digits[i] : (byte)0;

                    byte sum = (byte)(digitA + digitB + carry);
                    result.Add((byte)(sum % 10));
                    carry = (byte)(sum / 10);
                }
                if (carry > 0)
                    result.Add(carry);
                return new BigInteger(result);
            }
            public static BigInteger sub(BigInteger a, BigInteger b)
            {
                if (a < b)
                {
                    Console.WriteLine("The Second Integer Is Bigger Than The First One"); // The Requiered Class In The Task Only Supports Positive Nums
                    return new BigInteger(0);
                }

                return new BigInteger(sub2Lists(a.digits, b.digits));
            }
            public static BigInteger Mod(BigInteger a, BigInteger b)
            {
                if (a < b)
                    return new BigInteger(a);
                if (b == new BigInteger("0"))
                {
                    Console.WriteLine("Can Not Divide By 0           Maynf3sh :(");
                    return new BigInteger(0);
                }
                BigInteger temp = floor(a, b);
                BigInteger result = sub(a, Multiply(temp, b));
                return result;
            }
            public static BigInteger[] Div(BigInteger a, BigInteger b)
            {
                // Base case: if a is less than b, quotient is 0 and remainder is a
                if (a < b)
                    return new BigInteger[] { new BigInteger(0), a };

                // Calculate 2*b
                BigInteger twoB = Add(b, b);

                // Recursive call with a and 2*b
                BigInteger[] temp = Div(a, twoB);
                BigInteger q = temp[0];
                BigInteger r = temp[1];

                // Double the quotient (q = 2*q)
                q = Add(q, q);

                // Check and adjust remainder and quotient if needed
                if (r < b)
                    return new BigInteger[] { q, r };
                else
                    return new BigInteger[] { Add(q, new BigInteger(1)), sub(r, b) };
            }
            public static BigInteger Multiply(BigInteger a, BigInteger b)
            {
                // Check for zero multiplication
                if ((a.digits.Count == 1 && a.digits[0] == 0) || (b.digits.Count == 1 && b.digits[0] == 0))
                {
                    return new BigInteger("0");
                }

                // Simple multiplication  
                if (a.digits.Count <= 4 || b.digits.Count <= 4)
                {
                    List<byte> result = new List<byte>();
                    for (int i = 0; i < a.digits.Count + b.digits.Count; i++)
                    {
                        result.Add(0);
                    }

                    for (int i = 0; i < a.digits.Count; i++)
                    {
                        byte carry = 0;
                        for (int j = 0; j < b.digits.Count || carry > 0; j++)
                        {
                            byte digitB = j < b.digits.Count ? b.digits[j] : (byte)0;
                            int current = result[i + j] + a.digits[i] * digitB + carry;
                            result[i + j] = (byte)(current % 10);
                            carry = (byte)(current / 10);
                        }
                    }

                    return new BigInteger(result);
                }

                int n = Math.Max(a.digits.Count, b.digits.Count);
                int m = n / 2;

                // Split a into high and low parts
                List<byte> aLDigits = new List<byte>();
                List<byte> aHDigits = new List<byte>();

                for (int i = 0; i < a.digits.Count; i++)
                {
                    if (i < m)
                    {
                        aLDigits.Add(a.digits[i]);
                    }
                    else
                    {
                        aHDigits.Add(a.digits[i]);
                    }
                }

                if (aLDigits.Count == 0) aLDigits.Add(0);
                if (aHDigits.Count == 0) aHDigits.Add(0);

                BigInteger aL = new BigInteger(aLDigits);
                BigInteger aH = new BigInteger(aHDigits);

                // Split b into high and low parts
                List<byte> bLDigits = new List<byte>();
                List<byte> bHDigits = new List<byte>();

                for (int i = 0; i < b.digits.Count; i++)
                {
                    if (i < m)
                    {
                        bLDigits.Add(b.digits[i]);
                    }
                    else
                    {
                        bHDigits.Add(b.digits[i]);
                    }
                }

                if (bLDigits.Count == 0) bLDigits.Add(0);
                if (bHDigits.Count == 0) bHDigits.Add(0);

                BigInteger bL = new BigInteger(bLDigits);
                BigInteger bH = new BigInteger(bHDigits);

                // Karatsuba
                BigInteger z0 = Multiply(aL, bL);
                BigInteger z2 = Multiply(aH, bH);

                // z1 = (aLow + aHigh) * (bLow + bHigh) - z0 - z2
                BigInteger aS = Add(aL, aH);
                BigInteger bS = Add(bL, bH);
                BigInteger z1 = sub(sub(Multiply(aS, bS), z0), z2);

                // result = z2 * 10^(2*m) + z1 * 10^m + z0
                BigInteger res1 = new BigInteger(ShiftLeft(z2.digits, 2 * m));
                BigInteger res2 = new BigInteger(ShiftLeft(z1.digits, m));

                return Add(Add(res1, res2), z0);
            }

            private static List<byte> ShiftLeft(List<byte> digits, int shift)
            {
                if (digits.Count == 1 && digits[0] == 0)
                {
                    return new List<byte> { 0 };
                }

                List<byte> result = new List<byte>();

                for (int i = 0; i < shift; i++)
                {
                    result.Add(0);
                }

                result.AddRange(digits);

                return result;
            }
            private static List<byte> sub2Lists(List<byte> a, List<byte> b)
            {
                List<byte> finalRes = new List<byte>();
                int maxLength = Math.Max(a.Count, b.Count);
                byte borrow = 0;

                for (int i = 0; i < maxLength; i++)
                {
                    byte digitA = i < a.Count ? a[i] : (byte)0;
                    byte digitB = i < b.Count ? b[i] : (byte)0;

                    int diff = digitA - digitB - borrow;
                    if (diff < 0)
                    {
                        diff += 10;
                        borrow = 1;
                    }
                    else
                    {
                        borrow = 0;
                    }

                    finalRes.Add((byte)diff);
                }

                return finalRes;
            }
            public static BigInteger floor(BigInteger a, BigInteger b)
            {
                //BigInteger res = new BigInteger("0");
                BigInteger[] result = Div(a, b);
                return result[0];
            }
            // alm8arna
            public int compare(BigInteger num)
            { // 3amelha 3shan A3ml override ll Logical Operators
                if (this.digits.Count > num.digits.Count) return 1;
                if (this.digits.Count < num.digits.Count) return -1;

                for (int i = this.digits.Count - 1; i >= 0; i--)
                {
                    if (this.digits[i] > num.digits[i]) return 1;
                    if (this.digits[i] < num.digits[i]) return -1;
                }
                return 0;
            }

            //public override string ToString()
            //{
            //    string number = "";
            //    for (int i = digits.Count - 1; i >= 0; i--)
            //    {
            //        number += (char)(digits[i] + '0');
            //    }
            //    return number;
            //}


            public static BigInteger Sqrt(BigInteger N)
            {
                if (N.digits.Count == 1 && N.digits[0] == 0)
                    return new BigInteger("0");

                if (N.digits.Count == 1 && N.digits[0] == 1)
                    return new BigInteger("1");

                BigInteger l = new BigInteger("1");
                BigInteger h = new BigInteger(N.digits); // Copy constructor
                BigInteger finalRes = new BigInteger("0");

                while (l <= h)
                {
                    BigInteger mid = floor(Add(l, h), new BigInteger("2"));
                    BigInteger square = Multiply(mid, mid);

                    int cmp = square == mid ? 0 : -1;
                    if (cmp == 0)
                        return mid;

                    if (cmp < 0)
                    {
                        l = Add(mid, new BigInteger("1"));
                        finalRes = mid; // Store floor value
                    }
                    else
                    {
                        h = sub(mid, new BigInteger("1"));
                    }
                }

                return finalRes;
            }
    }
}
