namespace Plugin.Notification
{
    public interface ILocalNotification
    {

        /// <summary>
        /// Muestra una notificacion
        /// </summary>
        /// <param name="localnotification"></param>
        /// <returns></returns>
        bool Show(LocalNotification notification);

        /// <summary>
        /// Oculta una notificacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Hide(int id);

        /// <summary>
        /// Actualiza una notificacion, en caso de no existir, la lanza de nuevo
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        bool Update(LocalNotification notification);
    }
}