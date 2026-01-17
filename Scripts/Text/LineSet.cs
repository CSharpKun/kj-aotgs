using Godot;
using Godot.Collections;
using SteppeAttorney.Data;
using System.Collections.Generic;
using System.Linq;

namespace SteppeAttorney.Text;
public enum Placement
{
    First = 0,
    Second,
    Third,
    Fourth
}

/// <summary>
/// Simple LineSet that merges Character and their lines for consistency.
/// </summary>
[GlobalClass]
public partial class LineSet : Resource
{
    [Export] public Array<Line> Lines { get; set; }
    [Export] public Character CurrentCharacter { get; set; }
    [Export] public Placement Place { get; set; }
    public HashSet<int> LineOrders => Lines?
        .Where(l => l != null)
        .Select(l => l.Order)
        .ToHashSet() ?? [];
}
