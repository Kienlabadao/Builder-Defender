using System;

public interface IDamageable {
    public event EventHandler OnDamaged;

    public void Damage(float damageAmount);
}
