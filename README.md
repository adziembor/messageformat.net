# MessageFormatter for .NET

#### - better UI strings.

This is a fork of messageformat.net converted to regular .NET project with a bit changed pluralization part of the project.

Original project is an implementation of the ICU Message Format in .NET. For official information about the format, go to:
http://userguide.icu-project.org/formatparse/messages

## Quickstart

````csharp
var mf = new MessageFormatter();

var str = @"You have {notifications, plural,
              zero {no notifications}
               one {one notification}
               =42 {a universal amount of notifications}
             other {# notifications}
            }. Have a nice day, {name}!";
var formatted = mf.FormatMessage(str, new Dictionary<string, object>{
  {"notifications", 4},
  {"name", "Jeff"}
});

//Result: You have 4 notifications. Have a nice day, Jeff!

````

Or, if you don't like dictionaries, and don't mind a bit of reflection..

````csharp
var formatted = mf.FormatMessage(str, new {
  notifications = 0,
  name = "Jeff"
});

//Result: You have no notifications. Have a nice day, Jeff!
````

You can use a static method, too:

````csharp
var formatted = MessageFormatter.Format(str, new {
  notifications = 1,
  name = "Jeff"
});

//Result: You have one notification. Have a nice day, Jeff!
````

## Installation

Either clone this repo and build it, or install it with NuGet:

```
Install-Package MessageFormat
```

## Features

* **It's fast.** Everything is hand-written; no parser-generators, *not even regular expressions*.
* **It's portable.** The library is a PCL, and has just a single dependency ([Portable.ConcurrentDictionary](https://www.nuget.org/packages/Portable.ConcurrentDictionary/) for thread safety) - other than that the only reference is the standard `.NET` in PCL's.
* **It's compatible with other implementations.** I've been peeking a bit at the [MessageFormat.js][0] library to make sure
  the results would be the same.
* **It's (relatively) small**. For a .NET library, ~25kb is not a lot.
* **It's very white-space tolerant.** You can structure your blocks so they are more readable - look at the example above.
* **Nesting is supported.** You can nest your blocks as you please, there's no special structure required to do this, just ensure your braces match.
* **Adding your own formatters.** I don't know why you would need to, but if you want, you can add your own formatters, and
  take advantage of the code in my base classes to help you parse patterns. Look at the source, this is how I implemented the built-in formatters.
* **Exceptions make atleast a little sense.** When exceptions are thrown due to a bad pattern, the exception should include useful information.
* **There are unit tests.** Run them yourself if you want, they're using XUnit.
* **Built-in cache.** If you are formatting messages in a tight loop, with different data for each iteration, 
  and if you are reusing the same instance of `MessageFormatter`, the formatter will cache the tokens of each pattern (nested, too),
  so it won't have to spend CPU time to parse out literals every time. I benchmarked it, and on my monster machine, 
  it didn't make much of a difference (10000 iterations).

## Performance

If you look at `MessageFormatterCachingTests`, you will find a "with cache" and "without cache" test.

Author's machine runs on a Core i7 3960x, and with about **100,000** iterations with random data (generated beforehand), it takes about 2 seconds (1892ms) with the cache,
and about 3 seconds (3236ms) without it. **These results are with a debug build, when it is in release mode the time taken is reduced by about 40%! :)**

## Supported formats

Basically, it should be able to do anything that [MessageFormat.js][0] can do.

* Select Format: `{gender, select, male{He likes} female{She likes} other{They like}} cheeseburgers`
* Plural Format: `There {msgCount, plural, zero {are no unread messages} one {is 1 unread message} other{are # unread messages}}.` (where `#` is the actual number, with the offset (if any) subtracted).
* Simple variable replacement: `Your name is {name}`
 
## Adding your own pluralizer functions

Same thing as with [MessageFormat.js][0], you can add your own pluralizer function.
The `Pluralizers` property is a `IPluralizerCollection` - you can choose to
implement it on your own or use one of built-in ones (they are all used by default):
* `DictionaryPluralizerCollection` - just a regular case-insensitive dictionary
* `DefaultPluralizerCollection` - defaults, you can't modify them (they are initialized with the type)
* `OverlayingPluralizerCollection` - accepts two parameters - defaults and overlay, basically
allows you to use defaults with your own dictionary
* `FindingPluralizerCollection` - converts all lookup/addition strings from "iu-Latn-CA" to "iu_Latn_CA"
and allows you to do lookups with RFC1766 codes

````csharp
var mf = new MessageFormatter();
mf.Pluralizers.TryAddPluralizer("<locale>", n => {
  // ´n´ is the number being pluralized.
  if(n == 0)
    return "zero";
  if(n == 1)
    return "one";
  return "other";
});
````

There's no restrictions on what strings you may return, nor what strings
you may use in your pluralization block (unless you follow CLDR).

````csharp
var mf = new MessageFormatter(true, "en"); // true = use cache
mf.Pluralizers.TryAddPluralizer("en", n =>
{
    // ´n´ is the number being pluralized.
    if (n == 0)
        return "zero";
    if (n == 1)
        return "one";
    if (n > 1000)
        return "thatsalot";
    return "other";
};

mf.FormatMessage("You have {number, plural, thatsalot {a shitload of notifications} other {# notifications}}", new Dictionary<string, object>{
  {"number", 1001}
});
````

## Escaping literals

Simple - the literals are `{`, `}` and `#` (in a plural block). 
To escape a literal, use a `\` - e.g. `\{`.
  
# Bugs / issues

If you have issues with the library, and the exception makes no sense, please open an issue
and include your message, as well as the data you used.

# Todo

* Full pluralizations table generator with CLDR
* Ordinals (`{day}{day, selectordinal, one{st} two{nd} few{rd} many{rd} other{th}}`) support with CLDR rules

# Author

You can find the original author on Twitter: [@jeffijoe][1].

  [0]: https://github.com/SlexAxton/messageformat.js
  [1]: https://twitter.com/jeffijoe
