open System
open RabbitMQ.Client
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.ExchangeDeclare("topic_logs", "topic")

    let routingKey = argv.[0]
    let message = String.Join(" ", argv.[1])
    let body = Encoding.UTF8.GetBytes message

    channel.BasicPublish("topic_logs", routingKey, null, body)
    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
    Console.ReadKey() |> ignore
    0
