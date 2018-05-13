open System
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text
open System.Threading

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.QueueDeclare("task_queue", true, false, false, null) |> ignore
    channel.BasicQos(0u, 1us, false)

    
    let consumer = new EventingBasicConsumer(channel)

    let printReceivedMessage model (event:BasicDeliverEventArgs) =
        let body = event.Body
        let message = Encoding.UTF8.GetString body
        Console.WriteLine("Received message: {0}", message)

        let dots = message.Split(".") |> Array.length
        Thread.Sleep(dots * 1000)

        Console.WriteLine("Work done")
        channel.BasicAck(event.DeliveryTag, false)

    let receiveMessageEventHandler = new EventHandler<BasicDeliverEventArgs>(printReceivedMessage)
    consumer.Received.AddHandler(receiveMessageEventHandler)
    channel.BasicConsume("task_queue", false, consumer) |> ignore
    Console.WriteLine "Press any key to continue"
    Console.ReadKey() |> ignore
    0