# Authentication Solutions

Este repositório é dedicado ao estudo e implementação de estratégias de autenticação, focando especialmente em JWT (JSON Web Tokens) e OAuth. Os projetos aqui contidos demonstram a aplicação prática desses métodos em cenários reais, explorando suas características, vantagens e desafios.

## Sobre o Repositório

O objetivo deste repositório é oferecer uma visão abrangente sobre métodos modernos de autenticação, com exemplos práticos que podem ser utilizados como referência para desenvolvedores que desejam implementar sistemas de autenticação seguros e eficazes.

### Projeto Incluso

1. **JWT Authentication Project**
   - Uma implementação básica de autenticação usando JWT. Este projeto abrange a criação, validação e renovação de tokens JWT.

## Tecnologias Utilizadas

- .NET 8

## Funcionalidades

- **Geração de Token**: Implementação de endpoints que geram um JWT para os usuários após uma autenticação bem-sucedida.
- **Validação de Token**: Middleware que verifica a validade do JWT em cada solicitação protegida.
- **Refresh Tokens**: Experimentos com a implementação de refresh tokens para manter os usuários autenticados sem necessidade de reautenticação frequente.

# Explicação:

# JWT Authentication

JWT (JSON Web Token) é um padrão (RFC 7519) que define uma maneira compacta e independente de transmitir informações de forma segura entre as partes com um objeto JSON. Essas informações podem ser verificadas e validadas porque são assinadas digitalmente por secret-keys.

## Estrutura do JWT:

<pre>
<code>
<span style="color:red;">eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9</span>.<span style="color:blue;">eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ</span>.<span style="color:green;">SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</span>
</code>
</pre>


O JWT é dividido em 3 partes pelo “.” (ponto). A primeira parte é o header, a segunda parte é o payload e a terceira parte a signature.

1. Header : Contém metadados sobre o tipo do JWT e o algoritmo de assinatura usado, como HMAC SHA256 ou RSA, geralmente codificado em Base64
2. Payload: Contém as reivindicações(claims), que são declarações sobre uma entidade(geralmente o usuário) e qualquer dado adicional. Vale ressaltar que informações sensíveis, como senhas, não devem ser passadas no payload visto que essa informação pode ser lida por qualquer pessoa que possua o JWT. Nele existe um conjunto de reivindicações predefinidas que não são obrigatórias, mas recomendadas como `iss` (emissor), `exp`(tempo de expiração), `sub` (assunto do token), entre outras.
3. Signature: Para criar a assinatura, devemos pegar o header codificado, o payload codificado e assiná-los usando o algoritmo que foi declarado no header junto com o segredo. A assinatura é usada para identificar se a mensagem não foi alterada no trânsito. Mesmo que um usuário tenha o token e altere o JWT por exemplo: adicione uma role de “admin” o seu JWT não irá ser validado, visto que ele esse token que ele alterou não foi assinado pela secret-key, com isso o token esperado com o mesmo payload e header será diferente visto que a signature será diferente. 

## Utilização do JWT

JWTs são usados principalmente em autenticação e autorização:

- **Autenticação**: Após o usuário se logar, cada pedido subsequente incluirá o JWT, permitindo ao usuário acessar rotas, serviços e recursos que são permitidos com esse token.
- **Autorização**: O JWT facilita a verificação de que o cliente tem permissão para acessar um recurso específico.

## Validação e Segurança

- **Validação do Token**: O servidor precisa verificar a assinatura do token com a chave secreta (ou chave pública, para algoritmos assimétricos) para assegurar que o token é legítimo e não foi modificado.
- **Expiração**: O servidor deve verificar o campo `exp` para garantir que o token ainda é válido.
- **Renovação de Token**: Tokens podem ser renovados ou trocados por novos para manter a sessão do usuário ativa, uma prática que deve ser feita de forma segura para evitar brechas de segurança.

## Refresh Token

Um *refresh token* é um tipo especial de token usado para obter um novo *access token* após este expirar. Os *refresh tokens* são normalmente usados em conjunto com *access tokens* e são projetados para ter uma vida útil mais longa.

### Como Funciona o Processo de Refresh Token

1. **Login do Usuário**
    - O usuário se autentica no sistema usando suas credenciais.
    - O servidor valida as credenciais e emite um *access token* e um *refresh token*. O *access token* tem uma vida útil curta, enquanto o *refresh token* pode durar muito mais.
2. **Uso do Access Token**
    - O *access token* é usado para acessar recursos protegidos. Por ser de curta duração, minimiza o risco em caso de exposição.
3. **Quando o Access Token Expira**
    - Uma vez que o *access token* expira, o usuário não pode mais acessar recursos protegidos com ele.
4. **Solicitação de Novo Access Token**
    - Em vez de pedir ao usuário para se autenticar novamente, o sistema pode usar o *refresh token* para obter um novo *access token*.
    - O *refresh token* é enviado ao servidor, que valida o token e emite um novo *access token* (e, opcionalmente, um novo *refresh token*).
5. **Segurança do Refresh Token**
    - O *refresh token* deve ser armazenado de forma segura, pois permite acessar tokens de maneira contínua.
    - Deve-se implementar medidas para detectar e prevenir o uso indevido de *refresh tokens*, como limitar seu uso a um único dispositivo ou endereço IP, ou invalidá-lo se suspeitar de comprometimento.

### Boas Práticas para Uso de Refresh Tokens

- **Rotação de Refresh Tokens**: Sempre emita um novo *refresh token* com um novo *access token* para ajudar a prevenir o roubo e reutilização de tokens.
- **Armazenamento Seguro**: Armazene *refresh tokens* de forma segura para evitar acessos não autorizados.
- **Auditoria e Monitoramento**: Mantenha registros de quando os *refresh tokens* são usados para facilitar a detecção de atividades suspeitas.
- **Invalidação**: Permita que usuários invalidem *refresh tokens* ativos em caso de perda ou roubo de dispositivos.

## JWT vs Session

## Session (Stateful)

1. Usuário envia os dados de login para o servidor
2. O servidor então valida, cria uma sessão no banco de dados e depois responde com um ID de sessão
3. O ID da sessão será salvo no cookie do navegador(armazenamento chave-valor)
4. O navegador envia de volta ao servidor em cada nova solicitação subsequente esse valor armazenado.
5. O servidor então precisa ir ao banco de dados para verificar se a sessão é válida. O que é custoso, principalmente se precisamos escalar horizontalmente.

## JWT (Stateless)

1. Usuário envia os dados de login para o servidor
2. Servidor em vez de armazenar um ID de sessão ele gera um token web json, utilizando uma chave privada no servidor. Envia de volta para o navegador
3. No navegador ele será armazenado no localstorage
4. Em solicitações futuras o JWT será adicionado ao cabeçalho de autorização prefixado pela palavra: Bearer.  `Authorization: Bearer <token>.`
5. O servidor precisa apenas validar a assinatura com a chave privada. Não há necessidade de uma pesquisa no banco de dados ou em algum outro lugar.

OBS:

**Segurança**:

- **Sessão**: As sessões podem ser mais seguras, pois os dados sensíveis do usuário são mantidos no servidor e não no lado do cliente. Além disso, é mais fácil implementar mecanismos como rotação de sessão e invalidação de sessão.
- **JWT**: Embora o JWT seja seguro, ele pode ser vulnerável se o token for interceptado, pois todas as informações do usuário estão codificadas nele. Além disso, a gestão de tokens JWT expirados e a implementação de mecanismos de revogação são mais complexas.