# SFLang

### What is SFLang?

SFLang is a standalone language wrapper for SharpFunction, however currently it does not include any SharpFunction
related components.

### SFLang Rules (Lexic)

SFLang is based on LISP and some other examples i found online, which means it is based on symbols. One of the most
important characters in SFLang is '@'. It shows that anything that goes after it should not be instantiated.

Now lets get to actual Lexic.

## Defining variables

To define variable you must use 'let(@name, <value>)'. You can then set value to variable by doing 'set(
@name, <new value>)'.

One of the most unique parts of the SFLang is that you can set any type of data into a single variable, which means you
can set integer to already defined string etc.

Example:

```js
let(@int, 123)
set(@int, 542)

let(@string, "A string")
set(@string, 123) // an integer value :O
```

**NOTE: Since SFLang v0.6 you can now define variables like that:** 
```js
let @name = 123
@name = 542

let @another-var = "string"
// etc.
```
## Basic operators

There are 5 kinds of basic operators in SFLang

````js`
sum(first, second) // first + second
sub(first, second) // first - second
div(first, second) // first / second
mul(first, second) // first * second
mod(first, second) // first % second
pow(first, second) // first to power of second
```

## If/Else

That is how you can make if/else in SFLang:

```js
let @variable = true

if(variable, {
    // Code here
}, {
    // This is an alternative to else statements
})
```

**NOTE: Since SFLang v0.6 you can also do it like that:**

```js
let @cond = true

if(cond) {
    // code here
} else {
    // code here
}
```
There are still no 'else if' branches tho.
## Methods

One of interesting parts of SFLang is that it *does not have return statements*.

It means that anything at the end of a file, or at the end of a method will return last declared value.

Here is an example of method:

```js
let(@a-method, method({
    // This is a code inside a method
    
    // this value will be returned, because it is the last in a method
    150
})

// You can then call method like that:
a-method()
```

Lets see a method with parameters:

```js
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

You can invoke methods without certain params, and that will convert remaining ones to nulls

**NOTE: Since SFLang v0.6 you can do it like that too:**

```js
let @no-param = method() {
    out("Something")
}
no-param()
// > Something

let @with-param = method(@first, @second) {
    out(first)
    out(second)
}
with-param("Hello", "World")
// > Hello
// > World
```

**NOTE: Since SFLang v0.8 You no longer can delcare more than one method in a single file!**
This was made due to parsing errors.

## Booleans

In SFLang any value can be converted to boolean implicitly, which means that anything can be a bool in operation
requiring a bool.

It works the way, that if anything **is not null** it is **true** otherwise it is **false**

Example:

```js
let(@a-val, "true") // converted to true
let(@another-val, 514) // converted to true
let(@null-var, null) // converted to false!
```

You can operate with booleans with several main methods:

```js
not(a-boolean) // inverts the boolean, converting true to false and vice-versa
// or 
!a-boolean // <- works since SFLang v0.6

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

```js
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

```js
let @a-str = 'Cool string'
// length of string
length(a - str)
// replacing word in string
replace(a - str, 'Cool', 'Amazing')
// substring from string
substr(a - str, 1, 5)

// evaluating a string
eval('let(@evaluated, +(123,123,123))')
```

### Pointers
Pointers are type references, that can be *initialized*.

Initializing pointer creates a new element of pointer's referencing type.

Using pointers, you can invoke any functions from external .NET DLLs, and it doesn't even need to be in Mixin format!

Examples:

```js
// Creates new pointer, referencing to
// SFLang.AccessibleClass class
// NOTE: Pointers REQUIRE a semicolon
// at the end of pointer declaration!
// It is also recommended for pointers to be
// constants.
const @ptr => class SFLang.AccessibleClass;
// Method pointer
const @mtd => function SFLang.AccessibleClass.SomeMethod;

// You can put your initializer params here,
// if there are no params provided, you will receive
// object initialized with default provided empty
// constructor
let @instance = deref(ptr, ...)

// Working with instance

// Accessing .net field
let @field-value = field(instance, 'FieldName')

// Accessing .net property
let @property-value = prop(instance, 'PropertyName')

// Invoking methods
let @static-invoke = invoke-static(mtd)

// You can also supply your own parameters
let @default-invoke = invoke(mtd, instance, ...)

// Loading assemblies with pointers
// It loads the assembly and also returns loaded
// DLL so you can use it later
const @asm-ref => assembly ..SFLangCompiler.dll;

// Dereferencing items
const @nullref = ref(null)
const @stringref = ref("Some string")
const @intref = ref(123456) // Actually a .net long pointer
const @instanceref = ref(instance)
assert(eq(instanceref, ptr)) // They will be equal
```