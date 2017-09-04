using System;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Web.Http.Routing;
using SharedKernel.Api.Helpers;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Services;
using SharedKernel.Domain.ValueObjects;
using SharedKernel.DependencyInjection;

namespace SharedKernel.Api.Controllers
{
    public class QueryController<T> : ApiController where T : EntityBase
    {
        protected IQueryService<T> Service { get; set; }

        public QueryController()
        {
            Service = Kernel.Get<IQueryService<T>>();
        }

        /// <summary>
        /// Retorna uma entidade dado o seu ID
        /// </summary>
        /// <param name="id">ID da entidade</param>
        /// <returns>Entidade</returns>
        public virtual IHttpActionResult Get(long id)
        {
            try
            {
                if (id == 0)
                    return BadRequest("Informe o ID");

                var result = Service.Get(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Retorna uma lista com todas as entidades
        /// </summary>
        /// <returns>Lista de Entidades</returns>
        public virtual IHttpActionResult Get()
        {
            try
            {
                var result = Service.GetAll();
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Retorna uma lista paginada de entidades
        /// </summary>
        /// <param name="page">Número da Página Solicitada</param>
        /// <param name="size">Tamanho da Página</param>
        /// <returns>Lista Paginada de Entidades</returns>
        public virtual IHttpActionResult GetPage(int page, int size)
        {
            try
            {
                if (!Enum.IsDefined(typeof(PageSize), size))
                    throw new Exception("Tamanho de Página Inválido");

                var result = Service.GetPaged(page, (PageSize)size);

                if (result == null) return NotFound();

                var helper = new UrlHelper(Request);

                result.PreviousPage = page > 1 
                    ? helper.Link("DefaultApi", new { page = page - 1, size }) 
                    : string.Empty;

                result.NextPage = page < result.TotalPages 
                    ? helper.Link("DefaultApi", new { page = page + 1, size }) 
                    : string.Empty;

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Retorna uma Lista ODATA de Entidades
        /// </summary>
        /// <param name="queryOptions">Parametros do ODATA</param>
        /// <returns>Lista ODATA de Entidades</returns>
        public virtual IHttpActionResult GetOData(ODataQueryOptions<T> queryOptions)
        {
            try
            {
                var queryStringParts = ODataParse.RetonaQueryStringParts(queryOptions);
                var result = Service.GetOData(queryStringParts);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}