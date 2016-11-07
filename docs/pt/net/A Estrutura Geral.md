# **A Estrutura Geral**
No Grand Chase, os packets são primariamente divididos em três segmentos: _header_, _payload_ e _código de autenticação_. Vamos explicá-los um a um.

> Para fins de demonstração, usaremos o packet de reconhecimento SHA_FILENAME_LIST (ID: 0x001C).

Se tivéssemos _sniffado_ esse pacote, seu _buffer_ seria como este:

> ![](http://i.imgur.com/zbJ7iV4.png)

Analisemos suas partes:
## Header
> 6A 00 E7 8E 02 00 00 00 58 58 58 58 58 58 58 58

Em todos os packets, o header representa os 16 primeiros bytes dos dados recebidos. Ele contém algumas informações básicas a respeito do pacote em si, as quais serão explicadas detalhadamente abaixo.
> Nota: todos os dados no header são escritos no formato [little-endian](https://pt.wikipedia.org/wiki/Extremidade_(ordenação)).

### Tamanho
> ***6A 00*** E7 8E 02 00 00 00 58 58 58 58 58 58 58 58

Como o próprio nome sugere, esses dois bytes representam o tamanho total dos dados do pacote recebido.

O valor está no formato little-endian. Então, ele é, na verdade, _00 6A_, que é _106_ na forma decimal. Se você contar cada byte de nosso buffer, vai perceber que ele contém exatamente 106 bytes. :smiley:

### Prefixo
> 6A 00 ***E7 8E*** 02 00 00 00 58 58 58 58 58 58 58 58

Agora, deparamo-nos com o prefixo. 

Esses dois bytes estão presentes em todos os packets e contém um valor aleatório que é gerado no início da sessão e usado em todos os pacotes seguintes. Há apenas uma exceção: o pacote de definição das chaves da sessão, no qual o prefixo é representado por _00 00_ (esse pacote será discutido individualmente mais tarde).

### Contagem
> 6A 00 E7 8E ***02 00 00 00*** 58 58 58 58 58 58 58 58

A contagem é um inteiro de 4 bytes que representa a contagem de pacotes enviados dentro de uma sessão. Note que tanto o cliente quanto o servidor têm suas próprias contagens, ou seja, o servidor conta os pacotes enviados pelo servidor e o cliente conta os pacotes enviados pelo cliente.

No nosso caso, a contagem é 2, uma vez que ela é _00 00 00 02_ na forma hexadecimal com ordenação usual.

Assim como o prefixo, a contagem tem como exceção o mesmo packet (como dito, isso será explicado posteriormente).

### IV (Vetor de Inicialização)
> 6A 00 E7 8E 02 00 00 00 ***58 58 58 58 58 58 58 58***

É o IV usado na criptografia do payload do packet. 

Para cada pacote é gerado um IV que consiste em 8 bytes iguais que variam de _00_ a _FF_ em valores hexadecimais. Você deve dar uma olhada na [seção da criptografia](./A%20Criptografia.md#a-criptografia) para ter um melhor entendimento desse conceito.

## Payload (criptografado)
> CD 05 A5 3D 7B 8C 1D CD 03 15 B1 DE 85 36 72 D9 1F B6 03 7D 77 5A 01 BE 78 D4 0A 22 EB 63 BB D1 77 D2 C6 9F DB 17 BC 0A E2 CF D8 75 B2 9E 2E 30 DD 24 3E AA 3E 5B 90 FE 61 F2 C2 D1 05 A7 1C FD 9E 1B 69 A3 76 CE 3A 9D 69 21 21 9B 82 D7 00 DF

Localizado entre os 16 primeiros bytes (header) e os 10 últimos (código de autenticação), essa é a principal parte de um pacote.

À primeira vista, o payload está criptografado e não nos diz muito. Porém, quando decriptado, ele passa a representar os dados efetivos, aqueles que nos dizem algo realmente relevante como o login introduzido por um usuário ou a informação dos jogadores dentro de uma sala de missão. 

Devido à sua importância, o payload em si será discutido em sua [própria](./O%20Payload.md#o-payload) seção, do mesmo modo que a [criptografia](./A%20Criptografia.md#a-criptografia).

## Código de Autenticação (Auth Code)
> E3 57 33 57 A6 79 A3 F6 53 57

Representado pelos 10 últimos bytes dos dados, ele é a porção do pacote que é destinada a assegurar a autenticidade e integridade do resto. 

No Grand Chase, ele consiste em uma [MD5](https://pt.wikipedia.org/wiki/MD5)-[HMAC](https://pt.wikipedia.org/wiki/HMAC) (Hash-based Message Authentication Code).

> 6A 00 ***E7 8E 02 00 00 00 58 58 58 58 58 58 58 58 CD 05 A5 3D 7B 8C 1D CD 03 15 B1 DE 85 36 72 D9 1F B6 03 7D 77 5A 01 BE 78 D4 0A 22 EB 63 BB D1 77 D2 C6 9F DB 17 BC 0A E2 CF D8 75 B2 9E 2E 30 DD 24 3E AA 3E 5B 90 FE 61 F2 C2 D1 05 A7 1C FD 9E 1B 69 A3 76 CE 3A 9D 69 21 21 9B 82 D7 00 DF*** E3 57 33 57 A6 79 A3 F6 53 57

O cálculo do _auth code_ é feito com base na parcela do pacote destacada acima (do primeiro byte após o tamanho até o último byte do payload criptografado). Esse cálculo também usa uma chave de autenticação de 8 bytes que é definida no início da sessão de rede (isso será mais detalhado na [última seção](./O%20Inicio%20da%20Sessao.md#o-inicio-da-sessao)).

Normalmente, um MD5-HMAC teria o tamanho de 16 bytes. Entretanto, se olharmos o nosso pacote, perceberemos que seu código de autenticação tem apenas 10. Isso porque, no Grand Chase, o HMAC é truncado, sendo deixado com o tamanho de 10 bytes.
> ***E3 57 33 57 A6 79 A3 F6 53 57*** 10 17 F0 5F 40 F1

Em destaque, você pode ver a parcela do HMAC presente no pacote em relação ao HMAC inteiro calculado para aquele trecho dos dados do packet.

**Continue lendo**: [A Criptografia](./A%20Criptografia.md#a-criptografia)
