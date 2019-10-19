namespace ePhoto.NET.Helpers {
    public static class Patterns {
        public const string Slug = @"^([a-zA-Z0-9\u0600-\u06FF\uFB8A\u067E\u0686\u06AF]+(\-[a-zA-Z0-9\u0600-\u06FF\uFB8A\u067E\u0686\u06AF])*)*$";
        public const string AnyNumber = @"^([0-9]+|[1-9]+[0-9]+)$";
    }
}