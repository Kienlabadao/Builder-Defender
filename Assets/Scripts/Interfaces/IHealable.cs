using System;

public interface IHealable {
    public event EventHandler OnHealed;

    public void Heal(float healAmount);

    public void HealFull();
}
