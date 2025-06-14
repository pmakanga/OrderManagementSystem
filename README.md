# 📦 OrderManagementSystem API

## 🚀 Overview

This is a .NET 8 Web API project simulating an Order Management System. It includes:

- ✅ Discount calculation based on customer segment and order history  
- 🔁 Order status tracking with valid state transitions  
- 📊 Analytics endpoint for average order value and fulfillment time  
- 🧪 Unit and integration tests for core logic  
- 📄 Swagger/OpenAPI documentation  
- ⚡ Performance optimization using simple in-memory caching  
- 🧾 Basic customer and order creation via API

---

## 🧠 Assumptions

- The application uses an in-memory EF Core database for simplicity (no external DB needed).  
- Customer data can be added through the API or is pre-populated.  
- Each order is associated with an existing customer.  
- Discount rules are hardcoded for demonstration and vary by customer segment and order count.  
- Order status transitions follow business logic (e.g., Pending → Processing → Shipped → Delivered).  
- Analytics results are cached for 60 seconds to reduce repetitive computations.

---

## 🔧 Features

### 🏷️ Discounting System

- Applied based on:
  - Customer segment (Regular, Premium, VIP)
  - Minimum number of previous orders
- Example:
  - Premium customer with ≥ 3 orders gets a 10% discount.

---

### 🔄 Order Status Tracking

- Supported statuses:
  - `Pending`, `Processing`, `Shipped`, `Delivered`, `Cancelled`
- Valid transitions enforced:
  - `Pending → Processing`, `Processing → Shipped`, `Shipped → Delivered`
  - Any stage → `Cancelled`

---

### 📈 Order Analytics Endpoint

- Provides:
  - Average order value
  - Average fulfillment time (in minutes)
- Uses caching for performance (60-second validity window)

---

### 🧾 Customer & Order Creation

You can now create customers and orders via the API:

#### ➕ Create Customer
- **Endpoint:** `POST /api/customers`
- **Description:** Create a new customer with a specified segment
- **Request Body:**
  ```json
  {
    "segment": "Premium"
  }
  ```
- **Response:**
  ```json
  {
    "$id": "1",
    "id": 1,
    "segment": "Premium",
    "orders": null
  }
  ```

#### ➕ Create Order
- **Endpoint:** `POST /api/orders`
- **Description:** Create a new order for an existing customer
- **Request Body:**
  ```json
  {
    "customerId": 1,
    "totalAmount": 120
  }
  ```
- **Response:**
  ```json
  {
    "$id": "1",
    "id": 1,
    "customerId": 1,
    "totalAmount": 120,
    "createdAt": "2025-06-14T09:12:33.0553387Z",
    "fulfilledAt": null,
    "status": "Pending",
    "customer": {
      "$id": "2",
      "id": 1,
      "segment": "Premium",
      "orders": {
        "$id": "3",
        "$values": [
          {
            "$ref": "1"
          }
        ]
      }
    }
  }
  ```

Both endpoints are documented and testable via Swagger UI.

---

## 🧪 Testing

- **Unit tests** (`DiscountServiceTests.cs`)
  - Validates discount rules and business logic
- **Integration tests** (`OrdersControllerTests.cs`)
  - Covers API responses and edge cases (e.g., 404 on invalid order)

Run tests with:
```bash
dotnet test
```

---

## 📄 API Documentation

- **Swagger UI** available at: `/swagger`
- All endpoints include descriptive documentation for easy testing

---

## ▶️ Running the Application

1. **Clone the repository:**
   ```bash
   git clone https://github.com/pmakanga/OrderManagementSystem.git
   cd OrderManagementSystem
   ```

2. **Run the API:**
   ```bash
   dotnet run
   ```

3. **Open Swagger UI in your browser:**
   ```
   http://localhost:<port>/swagger
   ```

---

## 🗂️ Project Structure

```
OrderManagementSystem/
├── Controllers/           # API endpoints
├── Models/                # Domain models (Order, Customer, DiscountRule)
├── DTOs/                  # DTOs for creating Customer and Order
│   ├── OrderCreateDto.cs
│   └── CustomerCreateDto.cs
├── Services/              # Business logic and interfaces
├── Data/                  # EF Core context
├── Tests/                 # Unit & integration tests
├── Program.cs             # Entry point and DI setup
└── README.md              # This file
```

---

## 🛠️ Technologies Used

- **.NET 8** - Web API framework
- **Entity Framework Core** - In-memory database
- **Swagger/OpenAPI** - API documentation
- **XUnit** - Unit and integration testing
- **Memory Caching** - Performance optimization

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
