select  k.id,
        k.Naslov,
        BrojTema = count(kt.id)
        from Knjiznica.Knjiga K
        left join Knjiznica.KnjigaTema kt 
        on kt.KnjigaID = k.ID
        group by k.id, k.Naslov
    