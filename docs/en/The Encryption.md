# **The Encryption**
> Note: for better understanding, it's highly recommended that you read the content of the links provided in this section.

The payloads of the Grand Chase's packets are encrypted using the [DES algorithm](https://en.wikipedia.org/wiki/Data_Encryption_Standard) through the [CBC mode](https://en.wikipedia.org/wiki/Block_cipher_mode_of_operation#Cipher_Block_Chaining_.28CBC.29) (Cipher Block Chaining). 

The encryption process takes an _IV_, an 8-byte _key_ and a _plaintext_.
* As pointed before, an IV is generated for each packet and is sent together with it;
* The encryption key is defined at the start of the session like the auth key;
* The plaintext is the unencrypted payload.

Through the CBC mode, the data is processed in blocks of 8 bytes each. But what if the size of our data is not divisible by 8? That's what we will see below.

### Padding
> ![](https://i.imgur.com/85Nc0vm.png)

The whole thing you see above is the decrypted payload from our packet, but let's limit ourselves to the part in red for now.

This portion of the data is the _padding_. Its purpose is to fill the data until it reaches a length divisible by block size (in our case, 8).

Let's take our payload as example. Without padding, it would be 75 bytes long, but 75 is not divisible by 8. The next number divisible by 8 after 75 is 80, so our padding should be 5 bytes long (76 + 5 = 80). Then we start to count: 01, 02, 03, 04 and 04 again. The last byte of the padding is always equal to the penultimate byte. 

After this all, we now have ***01 02 03 04 04***: a 5-bytes long padding.

### The Padding Algorithm

As you may have thought, it would be impossible that the padding had the last byte equal to the penultimate if, for example, it was 1 byte long. Let's explain the algorithm a little better now.

It's actually very simple: when the distance from the payload length to the next number divisible by 8 is greater or equal to 2, the padding length will be this distance. When it is smaller, it will be the block size (8) plus the distance.

After this, the only step left is to write the padding's bytes themselves. This code snippet should be more enlightening:
```C#
// Calculates the distance between the length and the next value divisible by 8
int distance = 8 - (dataLength % 8);

if (distance >= 2)
{
  paddingLength = distance;
}
else
{
  paddingLength = 8 + distance;
}
for (byte i = 1; i < paddingLength; i++)
{
  padding[i - 1] = i;
}
padding[paddingLength - 1] = padding[paddingLength - 2];
```
And here's a table with all the 8 possible paddings for the payloads of the game to kill any remaining doubt:

| Distância          | Tamanho do Padding | Bytes do Padding                   |
| ------------------ | ------------------ | -----------------------------------|
| 0                  | 8                  | ***01 02 03 04 05 06 07 07***      |
| 1                  | 9                  | ***01 02 03 04 05 06 07 08 08***   |
| 2                  | 2                  | ***01 01***                        |
| 3                  | 3                  | ***01 02 02***                     |
| 4                  | 4                  | ***01 02 03 03***                  |
| 5                  | 5                  | ***01 02 03 04 04***               |
| 6                  | 6                  | ***01 02 03 04 05 05***            |
| 7                  | 7                  | ***01 02 03 04 05 06 06***         |

**Further reading**: [The Payload](./The%20Payload.md#the-payload)
