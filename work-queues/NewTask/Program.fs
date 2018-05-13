open System
open RabbitMQ.Client
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.QueueDeclare("task_queue", true, false, false, null) |> ignore
    let properties = channel.CreateBasicProperties()
    properties.Persistent <- true
    
    let message = String.Join(" ", argv)
    let body = Encoding.UTF8.GetBytes message

    channel.BasicPublish("", "task_queue", properties, body)
    Console.WriteLine("Sent message: {0}", message)
    Console.ReadKey() |> ignore
    0

