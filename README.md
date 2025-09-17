# OMS Integration Service

Servicio de integración que recibe eventos del TMS mediante **webhooks**, procesa reglas de negocio y notifica a clientes (Saga Falabella, Homecenter, Ripley).  
Implementado con **.NET 8** siguiendo una arquitectura **DDD simple**.

# Características principales

- Recepción de eventos desde TMS vía **Webhook (POST /api/webhook/tms)**.
- **Idempotencia**: cada evento se procesa una sola vez (trackingNumber + status + fecha).
- **Historial de eventos** (repositorio en memoria).
- **Actualización de pedidos (Order)** con sus reglas de negocio:
  - No modificar estados finales (`DELIVERED`, `RETURNED`).
  - Contador de intentos de entrega → si llega a 3 intentos fallidos, genera automáticamente `TO_BE_RETURN`.
- **Almacenamiento de evidencias** (fotos, firmas, etc.) en filesystem local (`./evidences/`), solo en los hitos:
  - `COLLECTED`, `NOT COLLECTED`, `DELIVERED`, `NOT DELIVERED`, `RETURNED`, `NOT RETURNED`.
- **Resiliencia** con [Polly] reintentos con backoff exponencial para descarga de evidencias.
- **Notificación a clientes con su infraestructura de cada una ** usando el patrón **Strategy/Adapter**:
  - Saga Falabella
  - Homecenter
  - Ripley


- **Domain**: Entidades (`Order`, `FailedEvidence`) e interfaces (`IOrderRepository`, `IEvidenceService`, `INotificationService`).  
- **Application**: Casos de uso (`EventProcessor`) que orquestan reglas de negocio.  
- **Infrastructure**: Repositorios en memoria, almacenamiento de evidencias local, adaptadores de notificación.  
- **API**: Controladores que exponen endpoints (webhook, evidencias fallidas).  

---

## Requisitos

- .NET 8 SDK  
- (Opcional) Docker si se quiere contenedizar  

---

## Cómo ejecutar

1. Clonar el repositorio
   ```bash
   git clone https://github.com/gianpvp99/OMS-IntegrationService.git
    cd OMS-IntegrationService
   dotnet run --project OMSIntegrationService.API
2. El servicio correrá en https://localhost:7245/swagger/index.html

## Ejemplo de usos (Probar con urls existentes)

## 1. Evento NOT DELIVERED

{
  "serviceType": "DELIVERY",
  "dispatchType": "NORMAL",
  "status": "NOT DELIVERED",
  "subStatus": "CUSTOMER_ABSENT",
  "vehicleCode": "TRUCK-123",
  "courierName": "Luis Ramirez",
  "eventDate": "2025-09-16T15:30:00Z",
  "details": {
    "orderNumber": "ORD-20250916-001",
    "trackingNumber": "ABC-999",
    "clientCode": "SAGA",
    "clientName": "Saga Falabella",
    "receivedBy": "",
    "comments": "Cliente no se encontraba en domicilio",
    "evidences": [
      {
        "label": "Foto Test Piero",
        "fileType": "image/png",
        "fileName": "Profesional_Piero.png",
        "url": "https://gianpierovasquez.com/assets/img/Profesional_Piero.png"
      }
    ]
  }
}

## Respuesta esperada: 

{
  "success": true,
  "message": "Event received",
  "trackingNumber": "ABC-999",
  "status": "NOT DELIVERED"
}

## EJEMPLO 2 DE 3 WEBHOOKS PARA EL MISMO PEDIDO ABC-999 CON 3 INTENTOS DE NOT DELIVERED

## INTENTO 1 (NOT DELIVERED)
{
  "status": "NOT DELIVERED",
  "subStatus": "CUSTOMER_ABSENT",
  "eventDate": "2025-09-16T15:30:00Z",
  "details": {
    "orderNumber": "ORD-20250916-001",
    "trackingNumber": "ABC-999",
    "comments": "Cliente no estaba en domicilio",
    "evidences": [
      {
        "fileName": "evidence1.jpg",
        "url": "https://www.turiweb.pe/wp-content/uploads/2021/07/pedidosya-070721.jpg"
      }
    ]
  }
}

## INTENTO 2 (NOT DELIVERED)

{
  "status": "NOT DELIVERED",
  "subStatus": "REFUSED",
  "eventDate": "2025-09-17T11:10:00Z",
  "details": {
    "orderNumber": "ORD-20250916-001",
    "trackingNumber": "ABC-999",
    "comments": "Cliente se negó a recibir el pedido",
    "evidences": [
      {
        "fileName": "evidence2.jpg",
        "url": "https://media-cdn.tripadvisor.com/media/photo-s/0c/89/20/c8/descampado-donde-podemos.jpg"
      }
    ]
  }
}

## INTENTO 3 (NOT DELIVERED) POR LO TANTO DISPARA "INTERNAMENTE" TO BE RETURNED 

{
  "status": "NOT DELIVERED",
  "subStatus": "ADDRESS_NOT_FOUND",
  "eventDate": "2025-09-18T09:45:00Z",
  "details": {
    "orderNumber": "ORD-20250916-001",
    "trackingNumber": "ABC-999",
    "comments": "Dirección inexistente",
    "evidences": [
      {
        "fileName": "evidence3.jpg",
        "url": "https://andro4all.com/hero/2025/03/dragon-tiny-home-genesis-portada.jpg"
      }
    ]
  }
}
