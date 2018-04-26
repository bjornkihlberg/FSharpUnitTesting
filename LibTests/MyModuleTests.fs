namespace LibTests

module MyModuleTests =
    open Lib.MyModule
    
    open Xunit
    open FsCheck
    open FsCheck.Xunit

    [<Property>]
    let f_test1 (x : int) : bool = f x > 0

    [<Property>]
    let f_test2 () : Property =
        let data = Gen.elements [1 .. 20] |> Arb.fromGen

        let test (x : int) : unit =
            Assert.True(f x > x)

        test |> Prop.forAll data

    [<Fact>]
    let f_test3 () : unit =
        Assert.Equal(f 4, 7)

    [<Theory>]
    [<InlineData(2, 4)>]
    [<InlineData(3, 6)>]
    [<InlineData(80, 160)>]
    [<InlineData(0, 0)>]
    [<InlineData(-7, -14)>]
    let f_test4 (x : int) (y : int) : unit =
        Assert.Equal(f x, y)
