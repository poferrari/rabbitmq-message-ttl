# rabbitmq-message-ttl
Projeto de estudo sobre Resiliência com RabbitMQ e Dead-Letter

# sobre o projeto
Aquivo docker-compose: `\rabbitmq-message-ttl\Docker`

Comando: `docker-compose up`

Acessar: http://localhost:15672/

Message.Publisher.Worker
- Aplicação responsável por publicar as mensagens na fila: `transactions-queue`

Message.Consumer.Worker
- Aplicação responsável por consumir as mensagens da fila: `transactions-queue`
- Simula uma exceção para que algumas mensagens sejam publicadas na fila: `transactions-queue-ttl`
- Após alguns minutos as mensagens da fila `transactions-queue-ttl` são publicadas na fila `transactions-queue` (tempo de expiração configurado)
- Há um limite de tentativas para tratar a mensagem, caso não consiga processar sem exceção, a mensagem sai da fila `transactions-queue`

# referências
https://lp.gago.io/rabbitmq?utm_source=gagoio&utm_campaign=template_rabbitmq&utm_medium=gagoio&utm_content=cta-lateral

https://www.wevo.io/implementando-resiliencia-entre-microservicos-com-dead-letter-e-rabbitmq/

https://medium.com/@leojasmim/delayed-exchanges-criando-workflow-de-mensagens-entre-filas-com-penalidade-em-rabbitmq-29392622fc77

https://www.veracode.com/blog/research/spring-rabbitmq-dead-letter-exchanges

https://zoltanaltfatter.com/2016/09/06/dead-letter-queue-configuration-rabbitmq/

https://renatogroffe.medium.com/net-core-2-1-asp-net-core-2-1-rabbitmq-exemplos-utilizando-mensageria-3e1427133167

https://renatogroffe.medium.com/mensageria-net-core-3-1-exemplos-com-rabbitmq-kafka-azure-service-bus-e-azure-queue-storage-c594bf30c091

https://www.youtube.com/watch?v=QzBvkZ4L1dg&list=PLfhPyyHRfeug87iBjkAP2ulwcqObiO_fW

https://gago.io/tag/rabbitmq/

# agradecimentos
- Paulo Sincos
- Pedro Verceze
- Vinicius Blasek
- Lucas Vieira
- Luiz Carlos Faria