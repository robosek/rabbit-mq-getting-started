open System
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.ExchangeDeclare("direct_logs", "direct")
    let queueName = channel.QueueDeclare() |> fun queue -> queue.QueueName

    argv |> Array.iter(fun severity -> channel.QueueBind(queueName, "direct_logs" ,severity))

    Console.WriteLine(" [*] Waiting for logs.");
    
    let consumer = new EventingBasicConsumer(channel)

    let printReceivedMessage model (event:BasicDeliverEventArgs) =
        let body = event.Body
        let severity = event.RoutingKey
        let message = Encoding.UTF8.GetString body
        Console.WriteLine(" [x] Received '{0}':'{1}'", severity, message)

    let receiveMessageEventHandler = new EventHandler<BasicDeliverEventArgs>(printReceivedMessage)
    consumer.Received.AddHandler(receiveMessageEventHandler)
    channel.BasicConsume(queueName, true, consumer) |> ignore
    Console.WriteLine "Press any key to exit"
    Console.ReadLine() |> ignore
    0