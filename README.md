# RSA SecureX

A C# implementation of the RSA public-key cryptosystem using custom big integer arithmetic for secure encryption/decryption of large numbers (hundreds of digits). The project includes extensions for string encryption and public key generation.

## Features

- **BigInteger Class**:
  - Supports addition, subtraction, multiplication, and division for arbitrarily large integers
  - Efficient algorithms (e.g., faster-than-naïve multiplication)
  - Odd/even checking

- **RSA Cryptosystem**:
  - Encryption: Computes `E(M) = M^e mod n`
  - Decryption: Computes `M = E(M)^d mod n`
  - String encryption/decryption support (Bonus Level 1)
  - Public key generation (Bonus Level 2):
    - Finds large pseudo-random primes p and q
    - Computes φ = (p-1)(q-1)
    - Selects e coprime to φ

- **Performance Metrics**:
  - Measures execution time for operations

- **File I/O**:
  - Processes test cases from input files
  - Outputs results with execution times

## Technologies

- **Language**: C#
- **Key Components**:
  - Custom `BigInteger` class
  - `System.Environment.TickCount` for timing
  - No external dependencies

## Getting Started

```bash
# Clone the repository
git clone https://github.com/yourusername/rsa-securex.git

# Open in Visual Studio and build  
