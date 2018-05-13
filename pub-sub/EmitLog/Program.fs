open System
open RabbitMQ.Client
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.ExchangeDeclare("logs", "fanout")

    let message = String.Join(" ", argv)
    let body = Encoding.UTF8.GetBytes message

    channel.BasicPublish("logs", "", null, body)
    Console.WriteLine("Sent message: {0}", message)
    Console.ReadKey() |> ignore
    0