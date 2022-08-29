using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Shared;
using AuthServer.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IGenericRepository<TEntity> genericRepository,IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var allEntities = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

            return Response<IEnumerable<TDto>>.Success(allEntities, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entitiy = await _genericRepository.GetByIdAsync(id);

            var mappedEntity = ObjectMapper.Mapper.Map<TDto>(entitiy);

            if (entitiy == null)
            {
                return Response<TDto>.Failure($"{id} not found", 404,true);
            }
            return Response<TDto>.Success(mappedEntity, 200);
        }

        public async Task<Response<NoDataDto>> RemoveAsync(int id)
        {
            var removedEntity = await _genericRepository.GetByIdAsync(id);

            if (removedEntity == null)
            {
                return Response<NoDataDto>.Failure($"{id} not found!", 404, true);
            }
            _genericRepository.Remove(removedEntity);
            //204 durum kodu => No Content => Response body'sinde hic bir data olmayacak.
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> UpdateAsync(TDto entity,int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Failure($"{id} not found!", 404, true);
            }
            var updatedEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            //204 durum kodu => No Content => Response body'sinde hic bir data olmayacak.
            _genericRepository.Update(updatedEntity);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);

        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            //where(x => x.id >5)
            var list =  _genericRepository.Where(predicate);
            var mappedList = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());
            //list.Take(5).Take(10); // ilk 5'i atla.ondan sonraki 10 urunu al.

            return Response<IEnumerable<TDto>>.Success(mappedList, 200);
        }
    }
}
