using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace VirtualLibrary.Interfaces;

public interface IMapper <T1, T2> {
    public T2 map(T1 obj);
    public T1 rmap(T2 robj);
    public ICollection<T2> lMap(ICollection<T1> lObj) {
        List<T2> t2s = new List<T2>();
        foreach (var elem in lObj) {
            t2s.Add(map(elem));
        }
        return t2s;
    }
    public ICollection<T1> lRMap(ICollection<T2> lRObj) {
        List<T1> t1s = new List<T1>();
        foreach (var elem in lRObj) {
            t1s.Add(rmap(elem));
        }
        return t1s;
    }

}