namespace EstoqueService.Data.JoinFacade
{
    public interface IJoinEntity<TEntity>
    {
        TEntity Navigation { get; set; }
    }
}

