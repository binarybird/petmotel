namespace PetMotel.Common.Rest
{
    public interface IResponse<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}