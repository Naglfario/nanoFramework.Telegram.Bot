# nanoFramework Client for Telegram Bot API

This client provides convenient access to the main functions of the Telegram Bot API:

- Sending messages
- Receiving updates (messages and button clicks)

Perhaps in the future, more methods for updating and deleting messages will be added,
but this is not certain. Basically, bots on embeded devices are needed to
control these devices, and for this, the simplest functions should be enough.

# Getting started

## Environment preparing

You can read more about usage examples in the Example project.

First, you need to connect to the Internet. You can do this, for example,
using [Wifi](https://github.com/nanoframework/System.Device.Wifi) library:

```csharp
const string wifiSSID = "YourSSID";
const string wifiPass = "YourWifiPassword";
 var isWifiConnected = WifiNetworkHelper.ConnectDhcp(wifiSSID, wifiPass,
 WifiReconnectionKind.Automatic, true, 0, CancellationToken.None);

if(!isWifiConnected)
{
    Debug.WriteLine("Wifi is not connected!");

    return;
}
```

Now you need to define [HttpClient](https://github.com/nanoframework/System.Net.Http)
with SSL certificates for Telegram.

To get certificates, you need to open any Telegram Bot API page in your browser and
download it manually(how to do this depends on each specific browser, find a way
for your browser on the internet). The important thing is that you need to get all
the certificates (currently there are 3), including the root ones. Generally you
can save ONLY the root certificates, and it will work, but it is less secure.
On the other hand, only the root certificates have a long validity period.
The final certificate will usually be valid for several months,
I doubt you want to reflash your device every few months to update the certificates,
and there is no more convenient way to get them. So I suggest taking only the root certificates.
You can open them with notepad and mark them one by one, in the order they were specified in the browser,
so that you get something like this:

```txt
-----BEGIN CERTIFICATE-----
MIIDxTCCAq2gAwIBAgIBADANBgkqhkiG9w0BAQsFADCBgzELMAkGA1UEBhMCVVMx
...
-----END CERTIFICATE-----
-----BEGIN CERTIFICATE-----
MIIE0DCCA7igAwIBAgIBBzANBgkqhkiG9w0BAQsFADCBgzELMAkGA1UEBhMCVVMx
...
-----END CERTIFICATE-----
```

So, you can paste it as string variable to your code, or you can add it to
[managed resources](https://github.com/nanoframework/nanoFramework.ResourceManager)
as text file and get it in code. I'll give you an example with managed resources:


```csharp
var httpClient = new HttpClient();
var certificates = Resources.GetString(Resources.StringResources.CertificatesTree);
httpClient.HttpsAuthentCert = new X509Certificate(certificates);
httpClient.SslProtocols = System.Net.Security.SslProtocols.Tls12;
```

If you want, you can skip the certificate fuss and just disable certificate checking
at the HttpClient level, it will work, but **I strongly advise against doing so**,
as your device will become significantly more vulnerable to man-in-the-middle attacks and others:

```csharp
// don't do this!
httpClient.SslVerification = System.Net.Security.SslVerification.NoVerification;
```

## Creating an Telegram Client

To create a client class you need http client created on previous step and
a Bot Token, you can get it from [Bot Father](https://t.me/BotFather):

```csharp
var token = "1234567890:0KLRiyDQv9C40LTQvtGAISEhISEhISEhISEhISEh";
var bot = new TelegramBot(TelegramBotToken, httpClient);
```


## Send a message

To send a message you need to create `SendTelegramMessageCommand` and pass it to `Send`
method of `TelegramBot`. For example:

```csharp
long recepientId = 1234567890;
bot.Send(new SendTelegramMessageCommand()
    {
        chat_id = recepientId,
        text = "Hi there!"
    });
```

`text` and `chat_id` fields are required, you can't send any messages without it.
However, these are not all the parameters. Check command class to see details.

## Check connection

In order to check if your bot is working, you can first call a special method:

```csharp
var connectionTest = bot.CheckConnection();

if (!connectionTest.ok)
{
    Debug.WriteLine($"Telegram connection error");
    Debug.WriteLine($"Error code: {connectionTest.error_code}");
    Debug.WriteLine($"Error description: {connectionTest.description}");

    WifiNetworkHelper.Disconnect();
    return;
}
```

This will result in the send of a `getMe` request to the Telegram API,
and you will receive a comprehensive answer whether everything is OK.
And if something went wrong, you will be able to see the reason.

## Receive updates

There are two ways to get updates: manually and automatically via events.

### Receive updates manually

To manually request updates when you need them, use the `GetUpdates` method:

```csharp
var updates = bot.GetUpdates();

if(updates.ProblemDetails != null)
{
    Debug.WriteLine(updates.ProblemDetails.Message);
    // looks like error was occured, make any actions that you need in this way
}

if(updates.RawUpdates != null && updates.RawUpdates.result != null)
{
    foreach(var update in updates.RawUpdates.result)
    {
        if(update.message != null)
        {
            // we received a message, handle it!
        }
        else if(update.callback_query != null)
        {
            // we received a button click, handle it!
        }
    }
}
```

### Receive updates automatically

To receive updates automatically, we have some [events](https://learn.microsoft.com/en-us/dotnet/standard/events/):

- An error occurred
- A message was received
- A button was pressed (callbackDate)

You can process them like this:

```csharp
bot.Events.OnError += (details) =>
{
    Debug.WriteLine($"OnError: {details.Message}");
};
bot.Events.OnMessageReceived += (message) =>
{
    // do something
};
bot.Events.OnCallbackQuery += (callbackQuery) =>
{
    // do something
};

bot.StartReceiving();
```

As you can see, to start receiving updates you need to call `StartReceiving`.
So, you can create methods for each event and pass it like this:

```csharp
public static void Main()
{
	// settings were omitted for the sake of compactness of the example, but they are necessary
    var bot = new TelegramBot(TelegramBotToken, httpClient);
    bot.Events.OnMessageReceived += Receive;
    bot.StartReceiving();

    Thread.Sleep(Timeout.Infinite);
}

public void Receive(TelegramMessage message)
{
	long adminId = 1234567890;
    if (message.from.id != adminId) return;

    if (!message.text.StartsWith("/"))
    {
        // it's not a command
    }
    else
	{
		// it's a command, handle it!
	}
}
```

### Stop receiving auto updates

To stop receiving automatic updates, you need to call the `StopReceiving` method:

```csharp
bot.StopReceiving();
```

## Working with inline buttons

By default, when a client requests updates from Telegram, it only asks messages.
Inline button clicks (`callbackData`) are not included in the request.
The thing is that clicks on these buttons are described by a rather heavy json,
and there is usually very little memory on embedded devices.
Therefore, it seems to me that if you can do without Inline Buttons,
it is better to do without them. Moreover, Telegram has another type of buttons (`ReplyKeyboard`),
clicking on which leads to sending a message to the chat (on behalf of user),
and the bot sees clicks on such buttons as new messages, and not as `callbackData`.

In addition, clicks on InlineButton lead to the fact that the pressed button begins to "blink",
and until the bot sends a separate http request to Telegram, which will say that the click has been received,
this button will blink. There is some limit, 10-15-20 seconds, and after reaching this limit,
the button stops blinking in any case, but still, this is not very pleasant.

However, despite all these disadvantages, this Telegram Bot Client supports receiving `CallbackData`.
To do this, you need to tell the bot that you want to receive such updates
(it is a pity to do this before calling the `StartReceiving` or `GetUpdates` methods):

```csharp
bot.ToggleCallbackDataUpdatesReceiving(true);
```

## Settings

This client has a few settings that can be changed at any time, each of them will be discussed below.

### Poll delay

This setting only makes sense if you are using `StartReceiving`.

This is a setting that determines how often the Telegram API will be polled for new updates.
The time is specified in milliseconds. Default value = `5000` (5 seconds)

```csharp
bot.UpdatePollDelay(10000);
```

I do not recommend setting this value too low, as this will result in different threads trying to
interact with HttpClient in a competitive mode, which is not allowed in nanoFramework.
Keep this value at a decent level or get updates manually via `GetUpdates`.

### Updates limit

This setting determines how many updates the client will try to get in one Http call.
Default value = 1. Maximum value = 100.

I do not recommend setting the value > 5, remember that this greatly consumes
the RAM of embedded devices, they do not have much of it.

```csharp
bot.UpdateLimit(2);
```

### Disable or enable message receiving

With this setting you can influence what types of updates will be requested
from Telegram on the next request. In particular, you can make it so
that telegram does not send you new messages. Default value = `true` (message updates are requested)

```csharp
bot.ToggleMessageReceiving(false);
```

### Disable or enable CallbackQuery receiving

By default, `CallbackQuery` is not requested from Telegram when we request updates,
for the reasons described above. However, you can change this:

```csharp
bot.ToggleCallbackDataUpdatesReceiving(true);
```

### Disable or enable CallbackQuery answers

As noted above, if user clicks on the InlineButton, the button starts blinking.
To stop this, you can either wait 10-15-20 seconds (but then it will look like
your bot is glitching to the user), or you need to confirm receipt of the click
by sending an additional request to the Telegram API.
This setting regulates whether such a request will be sent if the client receives CallbackData.
The default value is `true`, but this can be changed:

```csharp
bot.SetAnswerCallbackQuery(false);
```

### Disable or enable events for sending failures

When you send a message via the Telegram client, you receive a result
as `SendResult` class, which also contains information about the problems details.
However, if you want to make a centralized error handling algorithm,
you might want to receive error messages via the event system (`bot.Events.OnError`):

```csharp
bot.ToggleUseEventsForSendFailures(true);
```

## Q & A

### Q: Why do some property names look so strange? Why for example is the name `callback_data` used instead of `CallbackData`?

**A:** The reason is that it is difficult to work with JSON in nanoFramework.
The local `nanoFramework.Json` library does not yet have the ability to set
custom names for class properties via attributes like in the "big .NET".
Therefore, the names of the properties must be exactly the same as in the json.
There is an option to use deserialization ignoring the letter case,
but this is very expensive (the algorithm is several times slower)
and you still can't do anything with underscores.

### Q: Why this client doesn't have implementation of some endpoints?

**Short A**: I didn't need them.

**Right A:** If these are basic functions (delete, edit),
maybe they can be added in the future by me or the community.
Feel free to ask about the functions you need in Issues.