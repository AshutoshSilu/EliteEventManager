# Elite Event Management System - Folder Structure

## Backend (.NET Solution)

```
src/
├── EliteEvents.Domain/                    # Domain Layer (Core)
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── Permission.cs
│   │   ├── Customer.cs
│   │   ├── Employee.cs
│   │   ├── Vendor.cs
│   │   ├── VendorCategory.cs
│   │   ├── Venue.cs
│   │   ├── VenueImage.cs
│   │   ├── Event.cs
│   │   ├── EventCategory.cs
│   │   ├── EventImage.cs
│   │   ├── Package.cs
│   │   ├── PackageService.cs
│   │   ├── Booking.cs
│   │   ├── BookingDetail.cs
│   │   ├── Payment.cs
│   │   ├── Invoice.cs
│   │   ├── Review.cs
│   │   ├── Rating.cs
│   │   ├── Gallery.cs
│   │   ├── Notification.cs
│   │   ├── Testimonial.cs
│   │   ├── Coupon.cs
│   │   ├── Offer.cs
│   │   ├── AuditLog.cs
│   │   ├── Setting.cs
│   │   ├── Country.cs
│   │   ├── State.cs
│   │   ├── City.cs
│   │   ├── FAQ.cs
│   │   └── ContactMessage.cs
│   ├── Enums/
│   │   ├── BookingStatus.cs
│   │   ├── PaymentStatus.cs
│   │   ├── PaymentMethod.cs
│   │   ├── UserRole.cs
│   │   ├── EventStatus.cs
│   │   └── NotificationType.cs
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── IUnitOfWork.cs
│   │   ├── IUserRepository.cs
│   │   ├── IEventRepository.cs
│   │   ├── IBookingRepository.cs
│   │   ├── IVenueRepository.cs
│   │   ├── IVendorRepository.cs
│   │   ├── IPaymentRepository.cs
│   │   └── IReviewRepository.cs
│   └── Common/
│       ├── BaseEntity.cs
│       ├── AuditableEntity.cs
│       └── ISoftDeletable.cs
│
├── EliteEvents.Application/              # Application Layer
│   ├── DTOs/
│   │   ├── Auth/
│   │   ├── User/
│   │   ├── Event/
│   │   ├── Booking/
│   │   ├── Venue/
│   │   ├── Vendor/
│   │   ├── Payment/
│   │   ├── Review/
│   │   ├── Gallery/
│   │   ├── Notification/
│   │   └── Report/
│   ├── Services/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Validators/
│   ├── Mappings/
│   └── Common/
│       ├── PagedResult.cs
│       ├── ApiResponse.cs
│       └── QueryParameters.cs
│
├── EliteEvents.Infrastructure/           # Infrastructure Layer
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/
│   │   └── Migrations/
│   ├── Repositories/
│   ├── Services/
│   │   ├── EmailService.cs
│   │   ├── SmsService.cs
│   │   ├── PaymentService.cs
│   │   └── FileStorageService.cs
│   └── Identity/
│       ├── JwtTokenService.cs
│       └── PasswordHasher.cs
│
└── EliteEvents.API/                      # API Layer (Presentation)
    ├── Controllers/
    │   ├── AuthController.cs
    │   ├── UsersController.cs
    │   ├── EventsController.cs
    │   ├── BookingsController.cs
    │   ├── VenuesController.cs
    │   ├── VendorsController.cs
    │   ├── PaymentsController.cs
    │   ├── ReviewsController.cs
    │   ├── GalleryController.cs
    │   ├── NotificationsController.cs
    │   ├── PackagesController.cs
    │   ├── CouponsController.cs
    │   ├── ReportsController.cs
    │   └── SettingsController.cs
    ├── Middleware/
    │   ├── ExceptionMiddleware.cs
    │   └── RequestLoggingMiddleware.cs
    ├── Filters/
    │   └── ValidationFilter.cs
    ├── Extensions/
    │   └── ServiceExtensions.cs
    ├── Program.cs
    ├── appsettings.json
    └── appsettings.Development.json
```

## Frontend (Angular)

```
elite-events-frontend/
├── src/
│   ├── app/
│   │   ├── core/
│   │   │   ├── guards/
│   │   │   │   ├── auth.guard.ts
│   │   │   │   ├── role.guard.ts
│   │   │   │   └── no-auth.guard.ts
│   │   │   ├── interceptors/
│   │   │   │   ├── auth.interceptor.ts
│   │   │   │   ├── error.interceptor.ts
│   │   │   │   └── loading.interceptor.ts
│   │   │   ├── services/
│   │   │   │   ├── auth.service.ts
│   │   │   │   ├── token.service.ts
│   │   │   │   ├── notification.service.ts
│   │   │   │   └── theme.service.ts
│   │   │   ├── models/
│   │   │   │   ├── user.model.ts
│   │   │   │   ├── event.model.ts
│   │   │   │   ├── booking.model.ts
│   │   │   │   ├── venue.model.ts
│   │   │   │   ├── vendor.model.ts
│   │   │   │   ├── payment.model.ts
│   │   │   │   ├── review.model.ts
│   │   │   │   └── api-response.model.ts
│   │   │   └── constants/
│   │   │       ├── api-endpoints.ts
│   │   │       └── app-constants.ts
│   │   ├── shared/
│   │   │   ├── components/
│   │   │   │   ├── header/
│   │   │   │   ├── footer/
│   │   │   │   ├── sidebar/
│   │   │   │   ├── loading-spinner/
│   │   │   │   ├── confirm-dialog/
│   │   │   │   ├── data-table/
│   │   │   │   ├── pagination/
│   │   │   │   ├── breadcrumb/
│   │   │   │   ├── skeleton-loader/
│   │   │   │   └── image-upload/
│   │   │   ├── pipes/
│   │   │   │   ├── truncate.pipe.ts
│   │   │   │   ├── currency-format.pipe.ts
│   │   │   │   └── date-format.pipe.ts
│   │   │   ├── directives/
│   │   │   │   ├── has-role.directive.ts
│   │   │   │   └── click-outside.directive.ts
│   │   │   └── shared.module.ts
│   │   ├── features/
│   │   │   ├── auth/
│   │   │   │   ├── login/
│   │   │   │   ├── register/
│   │   │   │   ├── forgot-password/
│   │   │   │   └── reset-password/
│   │   │   ├── public/
│   │   │   │   ├── home/
│   │   │   │   ├── about/
│   │   │   │   ├── services/
│   │   │   │   ├── gallery/
│   │   │   │   ├── venues/
│   │   │   │   ├── events/
│   │   │   │   ├── packages/
│   │   │   │   ├── testimonials/
│   │   │   │   ├── contact/
│   │   │   │   ├── faq/
│   │   │   │   ├── privacy-policy/
│   │   │   │   └── terms-conditions/
│   │   │   ├── customer/
│   │   │   │   ├── dashboard/
│   │   │   │   ├── profile/
│   │   │   │   ├── bookings/
│   │   │   │   ├── wishlist/
│   │   │   │   ├── invoices/
│   │   │   │   ├── notifications/
│   │   │   │   ├── payments/
│   │   │   │   ├── reviews/
│   │   │   │   └── settings/
│   │   │   └── admin/
│   │   │       ├── dashboard/
│   │   │       ├── users/
│   │   │       ├── roles/
│   │   │       ├── events/
│   │   │       ├── categories/
│   │   │       ├── packages/
│   │   │       ├── venues/
│   │   │       ├── vendors/
│   │   │       ├── bookings/
│   │   │       ├── payments/
│   │   │       ├── gallery/
│   │   │       ├── testimonials/
│   │   │       ├── coupons/
│   │   │       ├── notifications/
│   │   │       ├── reviews/
│   │   │       ├── reports/
│   │   │       └── settings/
│   │   ├── layouts/
│   │   │   ├── public-layout/
│   │   │   ├── customer-layout/
│   │   │   └── admin-layout/
│   │   ├── app.component.ts
│   │   ├── app.config.ts
│   │   └── app.routes.ts
│   ├── assets/
│   │   ├── images/
│   │   ├── icons/
│   │   └── fonts/
│   ├── environments/
│   │   ├── environment.ts
│   │   └── environment.prod.ts
│   ├── styles/
│   │   ├── _variables.scss
│   │   ├── _mixins.scss
│   │   ├── _themes.scss
│   │   └── styles.scss
│   ├── index.html
│   └── main.ts
├── angular.json
├── package.json
└── tsconfig.json
```
