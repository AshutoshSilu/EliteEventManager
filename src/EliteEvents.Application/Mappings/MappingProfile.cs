using AutoMapper;
using EliteEvents.Application.DTOs.Booking;
using EliteEvents.Application.DTOs.Event;
using EliteEvents.Application.DTOs.Gallery;
using EliteEvents.Application.DTOs.Notification;
using EliteEvents.Application.DTOs.Payment;
using EliteEvents.Application.DTOs.Review;
using EliteEvents.Application.DTOs.User;
using EliteEvents.Application.DTOs.Vendor;
using EliteEvents.Application.DTOs.Venue;
using EliteEvents.Domain.Entities;

namespace EliteEvents.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping domain entities to DTOs and vice versa.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FirstName + " " + s.LastName))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name));
        CreateMap<User, UserProfileDto>()
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name));
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();

        // Event mappings
        CreateMap<Event, EventDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.AvailableSeats, o => o.MapFrom(s => s.AvailableSeats));
        CreateMap<Event, EventListDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.AvailableSeats, o => o.MapFrom(s => s.AvailableSeats));
        CreateMap<EventCreateDto, Event>();
        CreateMap<EventUpdateDto, Event>();
        CreateMap<EventImage, EventImageDto>();
        CreateMap<EventCategory, EventCategoryDto>();

        // Venue mappings
        CreateMap<Venue, VenueDto>()
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.City != null ? s.City.Name : null));
        CreateMap<Venue, VenueListDto>()
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.City != null ? s.City.Name : null));
        CreateMap<VenueCreateDto, Venue>();
        CreateMap<VenueUpdateDto, Venue>();
        CreateMap<VenueImage, VenueImageDto>();
        CreateMap<VenueAvailability, VenueAvailabilityDto>();

        // Vendor mappings
        CreateMap<Vendor, VendorDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.CityName, o => o.MapFrom(s => s.City != null ? s.City.Name : null));
        CreateMap<Vendor, VendorListDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
        CreateMap<VendorCreateDto, Vendor>();
        CreateMap<VendorUpdateDto, Vendor>();
        CreateMap<VendorCategory, VendorCategoryDto>();

        // Booking mappings
        CreateMap<Booking, BookingDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.User.FirstName + " " + s.Customer.User.LastName))
            .ForMember(d => d.CustomerEmail, o => o.MapFrom(s => s.Customer.User.Email))
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null))
            .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null))
            .ForMember(d => d.PackageName, o => o.MapFrom(s => s.Package != null ? s.Package.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<Booking, BookingListDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.User.FirstName + " " + s.Customer.User.LastName))
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null))
            .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<BookingDetail, BookingDetailDto>()
            .ForMember(d => d.VendorName, o => o.MapFrom(s => s.Vendor != null ? s.Vendor.BusinessName : null));

        // Payment mappings
        CreateMap<Payment, PaymentDto>()
            .ForMember(d => d.BookingNumber, o => o.MapFrom(s => s.Booking.BookingNumber))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.User.FirstName + " " + s.Customer.User.LastName))
            .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => s.PaymentMethod.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(d => d.BookingNumber, o => o.MapFrom(s => s.Booking.BookingNumber))
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.User.FirstName + " " + s.Customer.User.LastName))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        // Review mappings
        CreateMap<Review, ReviewDto>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.User.FirstName + " " + s.Customer.User.LastName))
            .ForMember(d => d.CustomerImage, o => o.MapFrom(s => s.Customer.User.ProfileImageUrl));
        CreateMap<ReviewCreateDto, Review>();

        // Gallery mappings
        CreateMap<Gallery, GalleryDto>()
            .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null));
        CreateMap<GalleryCreateDto, Gallery>();
        CreateMap<GalleryUpdateDto, Gallery>();

        // Notification mappings
        CreateMap<Notification, NotificationDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()))
            .ForMember(d => d.Channel, o => o.MapFrom(s => s.Channel.ToString()));
    }
}
