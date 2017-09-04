using System;
using System.Web.Http;
using System.Web.Http.Routing;
using SharedKernel.Api.Filters;
using SharedKernel.Api.Security;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Services;
using SharedKernel.Domain.Validation;
using SharedKernel.DependencyInjection;

namespace SharedKernel.Api.Controllers
{
    [UserAuthorization]
    public class CrudController<T> : QueryController<T> where T : EntityBase, IAggregateRoot
    {
        protected new ICrudService<T> Service { get; set; }

        public CrudController()
        {
            Service = Kernel.Get<ICrudService<T>>();
        }

        /// <summary>
        /// Incluí uma nova entidade (INSERT)
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <returns>Entidade inclusa</returns>
        public virtual IHttpActionResult Post([FromBody]T entity)
        {
            try
            {
                var token = ActionContext.RecuperarToken();

                Service.Insert(entity, token?.Login ?? "");

                var helper = new UrlHelper(Request);
                var location = helper.Link("DefaultApi", new { id = entity.Id });

                return Created(location, entity);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Atualiza dados da entidade (UPDATE)
        /// </summary>
        /// <param name="entity">Entidade</param>
        /// <returns>Entidade atualizada</returns>
        public virtual IHttpActionResult Put([FromBody]T entity)
        {
            try
            {
                var existe = Service.Get(entity.Id) != null;
                if (existe == false)
                    return NotFound();

                var token = ActionContext.RecuperarToken();
                Service.Update(entity, token?.Login);

                entity = Service.Get(entity.Id);

                return Ok(entity);
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Excluí a entidade (DELETE)
        /// </summary>
        /// <param name="id">ID da Entidade</param>
        /// <returns>OK</returns>
        public virtual IHttpActionResult Delete(long id)
        {
            try
            {
                var existe = Service.Get(id) != null;
                if (existe == false)
                    return NotFound();

                Service.Delete(id);

                return Ok();
            }
            catch (ValidatorException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}