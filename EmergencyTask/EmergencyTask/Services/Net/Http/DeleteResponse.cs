namespace Plugin.Net.Http
{
    public class DeleteResponse
    {
        public int id { get; set; }
        public int delete { get; set; }

        public bool HasBeenDeleted(int idtocomparrer)
        {
            return id == idtocomparrer && delete == 1;
        }
    }
}