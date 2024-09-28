namespace MicroServicioPersonas.Domain.Enums
{
    public enum MessageType
    {
        recetaRoutingKey,    // Clave de enrutamiento para recetas
        logRoutingKey,       // Clave de enrutamiento para logs
        Unknown    // Para mensajes con una routingKey no reconocida
    }
}