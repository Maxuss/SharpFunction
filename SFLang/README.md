# SFLang 
### What is SFLang?
SFLang is a standalone language wrapper for SharpFunction,
however currently it does not include any SharpFunction related components.

### SFLang Rules (Lexic)
SFLang is based on LISP and some other examples i found online, which means it is based on symbols.
One of the most important characters in SFLang is '@'.
It shows that anything that goes after it should not be instantiated.

Now lets get to actual Lexic.

## Defining variables
To define variable you must use 'let(@name, <value>)'.
You can then set value to variable by doing 'set(@name, <new value>)'.

One of the most unique parts of the SFLang is that you can set any type of data into a single variable, 
which means you can set integer to already defined string etc.

Example:
```c#
let(@int, 123)
set(@int, 542)

let(@string, "A string")
set(@string, 123) // an integer value :O
```

## Basic operators
There are 5 kinds of basic operators in SFLang

```c#
+(first, second) // first + second
-(first, second) // first - second
/(first, second) // first / second
*(first, second) // first * second
%(first, second) // first % second
```

## If/Else
That is how you can make if/else in SFLang:

```c#
set(@var, true)

if(var, {
    // Code here
}, {
    // This is an alternative to else statements
})
```

## Methods
One of interesting parts of SFLang is that it *does not have return statements*.

It means that anything at the end of a file, or at the end of a method will return last declared value.

Here is an example of method:

```c#
let(@a-method, method({
    // This is a code inside a method
    
    // this value will be returned, because it is the last in a method
    150
})

// You can then call method like that:
a_method()
```

Lets see a method with parameters:

```c#
let(@parametrised-method, method({
    out(@param-one)
    out(@param-two)
    
    // this is a 'void' method
    // it always returns 'null', because there 
    // is no default return value
}, @param-one, @param-two)

// And we can invoke it like that:

parametrised-method("First param", "Second param")
// output:
First param
Second param
```

## Booleans
In SFLang any value can be converted to boolean implicitly, which means that anything can be a bool in operation requiring a bool.

It works the way, that if anything **is not null** it is **true** otherwise it is **false**

Example:
```c#
let(@a-val, "true") // converted to true
let(@another-val, 514) // converted to true
let(@null-var, null) // converted to false!
```

You can operate with booleans with several main methods:

```c#
not(a-boolean) // inverts the boolean, converting true to false and vice-versa

// equality operators
eq(first, second) // true if both are the same
mt(first, second) // true if first is more than second
lt(first, second) // true if first is less than second
mte(first, second) // true if first is more or equal to second
lte(first, second) // true if first is less or equal to second

// note the '@' chars before values!
any(@first, @second) // true if first is true OR second is true
all(@first, @second) // true if first AND second are both true

```

## Collections
There are 2 kinds of collection in SFLang: list and map. Map is equal to dictionary in C# and list... is a list.

```c#
// list:
let(@a-list, list(123, 456, 789))
// counting elements in the list
let(@list-count, count(a-list)
// getting an element from the list
let(@list-element, get(a-list, 1)
// slices the list
let(@list-slice, slice(a-list, 0, 2) // from/to
// applies a method to the list
// basically unpacks the list, resulting in multiple arguments instead of a single one
+(apply(a-list))
// iterating through the list
each(@element, a-list {
    // code for iteration here
})

// map:
let(@a-map, map(
    'key1', 123,
    'key2', 456
))
// getting a value from map
let(@a-value, get(a-map, 'key1'))


// converting objects to json:
// to json:
let(@a-json, string(@a-value))
// from json:
let(@from-json, json('{"key":"value", "anotherKey":[1, 4, 16]}')
```

## Working with strings

```c#
let(@a-str, 'Cool string')
// length of string
length(a-str)
// replacing word in string
replace(a-str, 'Cool', 'Amazing')
// substring from string
substr(a-str, 1, 5)

// evaluating a string
eval('let(@evaluated, +(123,123,123))')

```