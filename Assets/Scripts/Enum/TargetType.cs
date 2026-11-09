using System;

[Flags]
public enum TargetType {
    Building = 1 << 1,
    Enemy = 1 << 2,
}
