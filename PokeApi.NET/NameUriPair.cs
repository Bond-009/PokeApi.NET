using System;

namespace PokeAPI.NET
{
    public struct NameUriPair
    {
        public string Name;
        public Uri ResourceUri;

        public NameUriPair(string name, Uri resourceUri)
        {
            Name = name;
            ResourceUri = resourceUri;
        }
        public NameUriPair(string name, string resourceUri)
            : this(name, new Uri(resourceUri))
        {

        }

        public override bool Equals(object obj)
        {
            return obj is NameUriPair && (NameUriPair)obj == this;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() | ResourceUri.GetHashCode();
        }
        public override string ToString()
        {
            return "{ Name: " + Name + ", ResourceUri: " + ResourceUri + "}";
        }

        public static bool operator ==(NameUriPair a, NameUriPair b)
        {
            return a.Name == b.Name && a.ResourceUri == b.ResourceUri;
        }
        public static bool operator !=(NameUriPair a, NameUriPair b)
        {
            return a.Name != b.Name || a.ResourceUri != b.ResourceUri;
        }
    }
    //public struct NameIDPair
    //{
    //    public string Name;
    //    public int ID;

    //    public NameIDPair(string name, int id)
    //    {
    //        Name = name;
    //        ID = id;
    //    }
    //}
    //public struct IDUriPair
    //{
    //    public Uri ResourceUri;
    //    public int ID;

    //    public IDUriPair(Uri resourceUri, int id)
    //    {
    //        ResourceUri = resourceUri;
    //        ID = id;
    //    }
    //    public IDUriPair(string resourceUri, int id)
    //        : this(new Uri(resourceUri), id)
    //    {

    //    }
    //}
}
