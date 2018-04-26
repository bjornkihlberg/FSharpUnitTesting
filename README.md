# Unit testing example in F#

## Summary
* Create console application as program starting point.
* Create library projects containing code to be tested.
* Create library projects for each library to be tested.
* Reference library projects containing implementation code in console application and in unit test libraries.
___
## Walkthrough
Lets say we have a software application and we want to unit test it. We keep the code to be unit tested in a library project (in this example called "**Lib**"). The console application "**App**" will refer to **Lib** to run the code. The unit tests will be implemented in a library project called "**LibTests**" that references **Lib**. **App** and **LibTests** have NOTHING to do with each other. This way no unit testing frameworks get into our application codebase.

**App** and **LibTests** references **Lib**.
![unittesting01](https://user-images.githubusercontent.com/38290734/38790855-85c70bf6-4176-11e8-9eca-70faad72e5bd.png)


### Add the following nuget packages to **LibTests**:
* *xunit* - [xUnit](https://xunit.github.io/t)
* *FsCheck.Xunit* - [FsCheck](https://fscheck.github.io/FsCheck/)
* *xunit.runner.visualstudio* for visual studio integration

### Steps
* Create a module (here named "**MyModule**") in **Lib** with some code to test.
![unittesting02](https://user-images.githubusercontent.com/38290734/38791309-7dc42490-4179-11e8-9f16-5d19aa913d7f.png)

* Create a module (here named "**MyModuleTests**") in **LibTests** for testing code in **MyModule**.
![unittesting03](https://user-images.githubusercontent.com/38290734/38791477-8b370e5c-417a-11e8-8b9b-668c4b500435.png)

* Build the solution to make code in **LibTests** see modules in **Libs**.

We can now access code in Lib from LibTests and run a test. This is a property test, it means that FsCheck will automatically find cases where the predicate will fail.
```fsharp
namespace LibTests

module MyModuleTests =
    open Lib.MyModule

    open FsCheck.Xunit

    [<Property>]
    let f_test (x : int) : bool = f x > 0
```

* Build **LibTests** and open the *Test Explorer* in Visual Studio and you should see something like this:
![unittesting04](https://user-images.githubusercontent.com/38290734/38870592-d82f7ce2-4280-11e8-898d-b9a32b3e6349.png)

* Right-click a unit test in the *Test Explorer* and select "Run Selected Tests" to run the test. For this particular test, FsCheck should find a case when `f x > 0` does not hold, i.e when x = 0.

Sometimes we only care about data to be tested in a particular range or in a particular sequence. We can define our own test data like this. More sophisticated test data can be generated like complex data structures.
```fsharp
namespace LibTests

module MyModuleTests =
    open Lib.MyModule
    
    open Xunit
    open FsCheck
    open FsCheck.Xunit

    [<Property>]
    let f_test () : Property =
        let data = Gen.elements [1 .. 20] |> Arb.fromGen

        let test (x : int) : unit =
            Assert.True(f x > x)

        test |> Prop.forAll data
```
*This time the test should pass since we're not including 0 in our test data.*

We can write a more classical style of example based test:
```fsharp
namespace LibTests

module MyModuleTests =
    open Lib.MyModule
    
    open Xunit

    [<Fact>]
    let f_test () : unit =
        Assert.Equal(f 4, 7)
```
This test should fail because `f 4` = 8, not 7.

We can define our test to use a couple of handcrafted test examples:
```fsharp
namespace LibTests

module MyModuleTests =
    open Lib.MyModule
    
    open Xunit

    [<Theory>]
    [<InlineData(2, 4)>]
    [<InlineData(3, 6)>]
    [<InlineData(80, 160)>]
    [<InlineData(0, 0)>]
    [<InlineData(-7, -14)>]
    let f_test (x : int) (y : int) : unit =
        Assert.Equal(f x, y)
```

More can be found on the project websites of xUnit and FsCheck.
