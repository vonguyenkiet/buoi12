-- Khởi tạo 3 schemas độc lập trong cùng 1 CSDL vật lý (Modular Monolithic Discipline)
CREATE SCHEMA IF NOT EXISTS "inventory";
CREATE SCHEMA IF NOT EXISTS "order";
CREATE SCHEMA IF NOT EXISTS "notification";

-- 1. Bảng Module Inventory
CREATE TABLE IF NOT EXISTS inventory.stocks (
    product_id UUID PRIMARY KEY,
    sku VARCHAR(50) NOT NULL,
    name VARCHAR(200) NOT NULL,
    price NUMERIC(18, 2) NOT NULL,
    available_quantity INT NOT NULL,
    reserved_quantity INT NOT NULL,
    sold_quantity INT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS inventory.processed_events (
    event_id UUID PRIMARY KEY,
    processed_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- 2. Bảng Module Order
CREATE TABLE IF NOT EXISTS "order".orders (
    id UUID PRIMARY KEY,
    customer_id UUID NOT NULL,
    status VARCHAR(20) NOT NULL,
    total_amount NUMERIC(18, 2) NOT NULL,
    failure_reason VARCHAR(500),
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS "order".order_items (
    id UUID PRIMARY KEY,
    order_id UUID NOT NULL REFERENCES "order".orders(id) ON DELETE CASCADE,
    product_id UUID NOT NULL,
    product_name_snapshot VARCHAR(200) NOT NULL,
    unit_price_snapshot NUMERIC(18, 2) NOT NULL,
    quantity INT NOT NULL
);

-- 3. Bảng Module Notification
CREATE TABLE IF NOT EXISTS notification.notification_logs (
    id UUID PRIMARY KEY,
    order_id UUID NOT NULL,
    event_id UUID NOT NULL,
    channel VARCHAR(20) NOT NULL,
    recipient VARCHAR(200) NOT NULL,
    message VARCHAR(1000) NOT NULL,
    is_simulation BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS notification.processed_events (
    event_id UUID PRIMARY KEY,
    processed_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);
