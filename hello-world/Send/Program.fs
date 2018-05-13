open System
open RabbitMQ.Client
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.QueueDeclare("hello",false, false, false, null) |> ignore
    
    let message = "hello world! 2"
    let body = Encoding.UTF8.GetBytes message

    channel.BasicPublish("","hello",null, body)
    Console.WriteLine("Sent message: {0}", message)
    Console.ReadKey() |> ignore
    0
