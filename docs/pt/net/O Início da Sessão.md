# **O Início da Sessão**

Nesta seção, estaremos discutindo algo muito importante: o início de uma sessão de rede de Grand Chase. Mais especificamente, trataremos do packet onde as chaves são definidas.

No Grand Chase, as chaves de criptografia e de autenticação para uma sessão são geradas aleatoriamente pelo servidor e enviadas para o cliente através do pacote de ID 0x0001, que é o primeiro pacote com alguma informação enviado.

Seus dados se pareceriam com estes:

> Nota: o packet aqui exposto é da season eternal. Eu não verifiquei os das outras seasons, mas eles podem ser diferentes.

```
52 00 00 00 31 18 00 00 59 59 59 59 59 59 59 59 91 98 7C 57 C3 D1 13 CE 9C 97 AC 0C 31 B0 79 78 DC AF 50 F4 F1 B0
61 9F 95 E3 0F DE 22 32 19 6A 86 FA B6 28 12 F4 1A E8 BC 40 02 84 0E 19 BF C6 46 26 3E 96 FA 52 6B 64 A2 3C 82 90
2C 9E 32 BB DA 8D
```
Primeiro, vamos falar sobre duas coisas: o _prefixo_ e a _contagem_

* **Prefixo**: no "primeiro packet", o prefixo não é aleatório. Ao invés disso, é sempre _00 00_;
* **Contagem**: no "primeiro packet", (obviamente) a contagem não mede a quantidade de pacotes enviados;

> Nota: eu ainda tenho minhas dúvidas sobre o que a contagem represente nesse packet. Ela não parece ser um valor aleatório. O que eu sei é que se ela for _00 00 00 00_, o cliente não o reconhece. É apenas uma questão de tempo até eu ter uma ideia mais clara sobre isso, eu só preciso analisar mais algumas amostras desse tipo de packet.

Você pode estar pensando: "Se as chaves da sessão estão dentro do payload encriptado, quais chaves foram usadas para criptografar os dados e gerar o HMAC desse pacote?"

Para esse packet, o Grand Chase usa duas chaves padrões que são armazenadas por ambos servidor e cliente:

* **Chave de Criptografia Padrão:** C7 D8 C4 BF B5 E9 C0 FD
* **Chave de Autenticação Padrão:** C0 D3 BD C3 B7 CE B8 B8

Tendo isso explicado, analisemos agora o payload do pacote.
> ![](http://i.imgur.com/nISFz3e.png?1)

Ele é como o de qualquer outro: tem um header, um conteúdo, e um preenchimento de bytes nulos ao final.

Os valores destacados são, respectivamente, as chaves de autenticação e criptografia definidas para o resto da sessão de rede. No nosso caso:

* **A parte em roxo é a chave de autenticação**: 79 F8 7C 96 04 9C 1E BE
* **A parte em vermelho é a chave de criptografia**: FA DE 9C F3 13 91 C8 38


E, por enquanto, isso é tudo. :smile:
