# Contribution Guidelines
Contributions to this SDK are encouraged and welcome.  Fork this repository and submit a [pull request][pull-requests-url].

# Code Style Guide
Please make sure all submitted code adheres to the following code style guide.

## Brace Style

Braces should be used in accordance with the [K&R style](https://en.wikipedia.org/wiki/Indentation_style#K&R_style). Opening braces should be placed at the end of the statement line.

```
namespace N {
    interface I {
        void Foo();
    }

    class C {
        void DoSomething() {
            if (condition) {
                Foo();
            }
        }
    }
}
```

---

## Declarations

Declarations should be organized so that members are easy to locate.

### Classes, Interfaces, & Enumerations

There should be exactly one class, interface, or enumeration per source file, except for situations in which properly scoped inner declarations are appropriate.

### Fields, Variables, & Properties

There should be one declaration per line.

---

## Nomenclature

Everything should be written using some variety of camel case barring a couple exceptions.


| Type         | Example    |
|--------------|------------|
| Camel Case   | camelCase  |
| Pascal Case  | PascalCase |
| Snake Case   | SNAKE_CASE |

> ? DO NOT prefix variables, properties, or fields with a single letter.

### Classes & Interfaces

Classes and Interfaces should be written in **pascal case**.

`class HomeSeerClass {}`

### Constants and Static Read-only Fields

Constants and static read-only fields should be written using **snake case**.

```
static readonly string FOO_BAR = "static";
const int HOME_SEER_TYPE = 11;
```

### Enumerations

Enums should be written using **pascal case** and prefixed with an **E**.

`enum EDeviceType {}`

Enum members should be written using **pascal case**.

```
enum ELogType {
    Info,
    Warning,
    Error
}
```

### Events

Events should be written in **pascal case**.

`public event SampleEventHandler SampleEvent;`

### Fields

Field nomenclature depends on scope.  All fields should be written in **pascal case** except for private fields.

```
protected int Counter;
public string Description;
```

#### Private Fields

Private fields should be written in **camel case** with a leading underscore ( **_** ).

`private string _uniqueKey;`

### Interfaces

All interfaces should written using **pascal case** and be prefaced with the letter **I**.

`interface IEventListener {}`

### Local Variables and Constants

Local variables and constants should be written in **camel case**. Local scope often refers to the body of a method.

```
string tempKey;
const int typeKey = -42;
```

> ? Single character variables and parameter names should be avoided except for temporary looping variables.

### Methods

Methods should be written in **pascal case**.

`void DoSomething() {}`

### Namespaces

Namespaces should be written in **pascal case**.

`namespace HomeSeer {}`

### Parameters

Parameters should be written using **camel case**.

`void DoSomething(string firstParam, int secondParam) {}`

### Properties

Properties should be written in **pascal case**.

`public string Description { get; set; }`

### Type Parameters

Type parameters should be written using **pascal case** and be prefixed with a **T**.

`class Foo<TFooParameter> {}`

---

## Tabs, Indents, & Alignments

Indents should be made up of 4 spaces for consistency across platforms. If the IDE supports it, configure the tab width to 4 spaces so you can use tab.

### Nested Statements

Always indent nested statements.

```
foreach (var a in x)
    foreach (var b in y)
        foreach (var c in z) {
            foo();
        }
    }
}
```

```
if (a == b) {
    if (c == d) {
        foo();
    }
}
```

### Parenthesis

Use a continuous line indent inside parenthesis and angle brackets to align the content within if it needs to wrap to a new line.
Use the BSD/K&R style and align the closing parenthesis or angle bracket with the start of the line the opening one is on. If everything fits on one line, do not wrap the content.

```
void Method(
    int parameter1,
    int parameter2
    ) {
        Foo();
    }
```

```
var x = Method<
    Class1,
    Class2
> ();
```

### Preprocessor Directives

? DO NOT indent any preprocessor directives except for #region and #endregion.

```
namespace N {
    class C {
#if !HideSomething
        int myField;
#endif
        #region Fields
        int myField2;
        #endregion
    }
}
```

### Other Indents

? DO indent case statements from their containing switch.

```
switch (expression) {
    case 0:
        break;
}
```

? DO NOT outdent statement labels

```
{
    int a = 5;
    MyLabel:
    a--;
    if (a > 0) goto MyLabel;
}
```

? DO indent type constraints

```
class C1<T1>
    where T1 : I1 {
}
```

? DO NOT indent comments started at the first column

```
namespace N {
// Some comment
    class C {
    }
}
```

? DO NOT place comments at the first column when commenting out code. Comments should use the indentation level of the commented code.

? DO indent braces inside statement conditions

```
while (x is IMyInterface {
            Prop1: 1,
            Prop2: 2
        }) {
    DoSomething();
}
```

### Align Multiline Constructs

In general, do not align multi-line statements. Some statement structures should be aligned to allow for them to be more easily identified at a glance.

? DO align the parts of a multi-line LINQ query

```
var q = from x in xs
        where x != null
        select x;
```

? DO align the parts of a multi-line binary expression or pattern

```
var a = someOperand + operand2
                    + operand3
                    + operand4;
```

? DO NOT align multi-line chained method calls

```
MyVar.SomeMethod()
    .OtherMethod()
    .ThirdMethod();
```

? DO NOT align array, object, and collection initializers

```
StudentName student = new StudentName {
    FirstName = "John",
    LastName = "Smith",
    ID = 116
}
```

? DO NOT align an anonymous method body with its statement

```
FooCall(delegate {
    DoSomething();
    return 0;
});
```

? DO align statement conditions inside parenthesis

```
while (x is IMyInterface or
            IMyInterface2 or
            IMyInterface3 {
                Prop1: 1,
                Prop2: 2
            }) {
    DoSomething();
}
```

### Align Similar Code in Columns

? DO NOT align similar code in columns. It makes it look nice, but it makes it difficult to edit.

-----


[pull-requests-url]: https://github.com/HomeSeer/Plugin-SDK/pulls