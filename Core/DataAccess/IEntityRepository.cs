using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
	// generic constraint - generic kısıtlama
	// class : reference tip olabilir demek
	// IEntity olabilir veya IEntity impelemente eden bir class olabilir.
	// new() : new'lenebilir olmalı (IEntity yazmayalım diye)
	public interface IEntityRepository<T> where T:class, IEntity, new()
	{
		List<T> GetAll(Expression<Func<T,bool>> filter=null);
		T Get(Expression<Func<T, bool>> filter);
		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
