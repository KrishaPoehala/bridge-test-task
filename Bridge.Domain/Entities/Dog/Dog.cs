using Bridge.Domain.Common;
using System.Runtime.Intrinsics.X86;
using System;

namespace Bridge.Domain.Entities.Dog;

public class Dog
{
    public DogId Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int TailLength { get; set; }
    public int Weight { get; set; }
}