CREATE TABLE "authorizationModels"
(
    id                   uuid   NOT NULL,
    "apiKeyHash"         text   NOT NULL,
    owner                text   NOT NULL,
    "availableServices"  text[] NOT NULL,
    "availableResources" text[] NOT NULL,
    "createdUtcTicks"    bigint NOT NULL,
    "expirationUtcTicks" bigint,
    CONSTRAINT "PK_authorizationModels" PRIMARY KEY (id)
);
