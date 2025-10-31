CREATE TABLE IF NOT EXISTS recipient_account
(
    id               uuid                     NOT NULL,
    login            text                     NOT NULL,
    password_hash    text                     NOT NULL,
    account_state_id uuid                     NOT NULL,
    timezone_info_id text                     NOT NULL,
    created_at_utc   timestamp with time zone NOT NULL,
    updated_at_utc   timestamp with time zone NOT NULL,
    CONSTRAINT "PK_recipient_account" PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS recipient_account_state
(
    id           uuid    NOT NULL,
    state        integer NOT NULL,
    state_reason integer NOT NULL,
    CONSTRAINT "PK_recipient_account_state" PRIMARY KEY (id)
);