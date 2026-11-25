public interface IHasHealth {
    public float GetHealthAmountMax();

    public float GetHealthAmount();

    public float GetMissingHealthAmount();

    public bool IsFullHealth();
}
