using System;

public struct WallKickStateChange
{
    public int InitialRotation { get; }
    public int DesiredRotation { get; }

    public WallKickStateChange(int initialRotation, int desiredRotation) => (InitialRotation, DesiredRotation) = (initialRotation, desiredRotation);

    public override bool Equals(object obj) => obj is WallKickStateChange o && Equals(o);

    public bool Equals(WallKickStateChange other) => InitialRotation == other.InitialRotation && DesiredRotation == other.DesiredRotation;

    public override int GetHashCode() => HashCode.Combine(InitialRotation, DesiredRotation);
}
