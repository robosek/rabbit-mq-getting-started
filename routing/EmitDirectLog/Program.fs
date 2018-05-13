open System
open RabbitMQ.Client
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.ExchangeDeclare("direct_logs", "direct")

    let severity = argv.[0]
    let message = String.Join(" ", argv.[1])
    let body = Encoding.UTF8.GetBytes message

    channel.BasicPublish("direct_logs", severity, null, body)
    Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
    Console.ReadKey() |> ignore
    0